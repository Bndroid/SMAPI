using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StardewModdingAPI.Toolkit;
using StardewModdingAPI.Toolkit.Framework.Clients.WebApi;
using StardewModdingAPI.Toolkit.Framework.Clients.Wiki;
using StardewModdingAPI.Toolkit.Framework.ModData;
using StardewModdingAPI.Web.Framework.Clients.Chucklefish;
using StardewModdingAPI.Web.Framework.Clients.GitHub;
using StardewModdingAPI.Web.Framework.Clients.Nexus;
using StardewModdingAPI.Web.Framework.ConfigModels;
using StardewModdingAPI.Web.Framework.ModRepositories;

namespace StardewModdingAPI.Web.Controllers
{
    /// <summary>Provides an API to perform mod update checks.</summary>
    [Produces("application/json")]
    [Route("api/v{version:semanticVersion}/mods")]
    internal class ModsApiController : Controller
    {
        /*********
        ** Properties
        *********/
        /// <summary>The mod repositories which provide mod metadata.</summary>
        private readonly IDictionary<string, IModRepository> Repositories;

        /// <summary>The cache in which to store mod metadata.</summary>
        private readonly IMemoryCache Cache;

        /// <summary>The number of minutes successful update checks should be cached before refetching them.</summary>
        private readonly int SuccessCacheMinutes;

        /// <summary>The number of minutes failed update checks should be cached before refetching them.</summary>
        private readonly int ErrorCacheMinutes;

        /// <summary>A regex which matches SMAPI-style semantic version.</summary>
        private readonly string VersionRegex;

        /// <summary>The internal mod metadata list.</summary>
        private readonly ModDatabase ModDatabase;

        /// <summary>The web URL for the wiki compatibility list.</summary>
        private readonly string WikiCompatibilityPageUrl;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="environment">The web hosting environment.</param>
        /// <param name="cache">The cache in which to store mod metadata.</param>
        /// <param name="configProvider">The config settings for mod update checks.</param>
        /// <param name="chucklefish">The Chucklefish API client.</param>
        /// <param name="github">The GitHub API client.</param>
        /// <param name="nexus">The Nexus API client.</param>
        public ModsApiController(IHostingEnvironment environment, IMemoryCache cache, IOptions<ModUpdateCheckConfig> configProvider, IChucklefishClient chucklefish, IGitHubClient github, INexusClient nexus)
        {
            this.ModDatabase = new ModToolkit().GetModDatabase(Path.Combine(environment.WebRootPath, "StardewModdingAPI.metadata.json"));
            ModUpdateCheckConfig config = configProvider.Value;
            this.WikiCompatibilityPageUrl = config.WikiCompatibilityPageUrl;

            this.Cache = cache;
            this.SuccessCacheMinutes = config.SuccessCacheMinutes;
            this.ErrorCacheMinutes = config.ErrorCacheMinutes;
            this.VersionRegex = config.SemanticVersionRegex;
            this.Repositories =
                new IModRepository[]
                {
                    new ChucklefishRepository(config.ChucklefishKey, chucklefish),
                    new GitHubRepository(config.GitHubKey, github),
                    new NexusRepository(config.NexusKey, nexus)
                }
                .ToDictionary(p => p.VendorKey, StringComparer.CurrentCultureIgnoreCase);
        }

        /// <summary>Fetch version metadata for the given mods.</summary>
        /// <param name="model">The mod search criteria.</param>
        [HttpPost]
        public async Task<object> PostAsync([FromBody] ModSearchModel model)
        {
            // parse request data
            ISemanticVersion apiVersion = this.GetApiVersion();
            ModSearchEntryModel[] searchMods = this.GetSearchMods(model, apiVersion).ToArray();

            // fetch wiki data
            WikiCompatibilityEntry[] wikiData = await this.GetWikiDataAsync();

            // fetch data
            IDictionary<string, ModEntryModel> mods = new Dictionary<string, ModEntryModel>(StringComparer.CurrentCultureIgnoreCase);
            foreach (ModSearchEntryModel mod in searchMods)
            {
                if (string.IsNullOrWhiteSpace(mod.ID))
                    continue;

                ModEntryModel result = await this.GetModData(mod, wikiData, model.IncludeExtendedMetadata);
                result.SetBackwardsCompatibility(apiVersion);
                mods[mod.ID] = result;
            }

            // return in expected structure
            return apiVersion.IsNewerThan("2.6-beta.18")
                ? mods.Values
                : (object)mods;
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Get the metadata for a mod.</summary>
        /// <param name="search">The mod data to match.</param>
        /// <param name="wikiData">The wiki data.</param>
        /// <param name="includeExtendedMetadata">Whether to include extended metadata for each mod.</param>
        /// <returns>Returns the mod data if found, else <c>null</c>.</returns>
        private async Task<ModEntryModel> GetModData(ModSearchEntryModel search, WikiCompatibilityEntry[] wikiData, bool includeExtendedMetadata)
        {
            // resolve update keys
            var updateKeys = new HashSet<string>(search.UpdateKeys ?? new string[0], StringComparer.InvariantCultureIgnoreCase);
            ModDataRecord record = this.ModDatabase.Get(search.ID);
            if (record?.Fields != null)
            {
                string defaultUpdateKey = record.Fields.FirstOrDefault(p => p.Key == ModDataFieldKey.UpdateKey && p.IsDefault)?.Value;
                if (!string.IsNullOrWhiteSpace(defaultUpdateKey))
                    updateKeys.Add(defaultUpdateKey);
            }

            // get latest versions
            ModEntryModel result = new ModEntryModel { ID = search.ID };
            IList<string> errors = new List<string>();
            foreach (string updateKey in updateKeys)
            {
                // fetch data
                ModInfoModel data = await this.GetInfoForUpdateKeyAsync(updateKey);
                if (data.Error != null)
                {
                    errors.Add(data.Error);
                    continue;
                }

                // handle main version
                if (data.Version != null)
                {
                    if (!SemanticVersion.TryParse(data.Version, out ISemanticVersion version))
                    {
                        errors.Add($"The update key '{updateKey}' matches a mod with invalid semantic version '{data.Version}'.");
                        continue;
                    }

                    if (this.IsNewer(version, result.Main?.Version))
                        result.Main = new ModEntryVersionModel(version, data.Url);
                }

                // handle optional version
                if (data.PreviewVersion != null)
                {
                    if (!SemanticVersion.TryParse(data.PreviewVersion, out ISemanticVersion version))
                    {
                        errors.Add($"The update key '{updateKey}' matches a mod with invalid optional semantic version '{data.PreviewVersion}'.");
                        continue;
                    }

                    if (this.IsNewer(version, result.Optional?.Version))
                        result.Optional = new ModEntryVersionModel(version, data.Url);
                }
            }

            // get unofficial version
            WikiCompatibilityEntry wikiEntry = wikiData.FirstOrDefault(entry => entry.ID.Contains(result.ID.Trim(), StringComparer.InvariantCultureIgnoreCase));
            if (wikiEntry?.UnofficialVersion != null && this.IsNewer(wikiEntry.UnofficialVersion, result.Main?.Version) && this.IsNewer(wikiEntry.UnofficialVersion, result.Optional?.Version))
                result.Unofficial = new ModEntryVersionModel(wikiEntry.UnofficialVersion, this.WikiCompatibilityPageUrl);

            // fallback to preview if latest is invalid
            if (result.Main == null && result.Optional != null)
            {
                result.Main = result.Optional;
                result.Optional = null;
            }

            // special cases
            if (result.ID == "Pathoschild.SMAPI")
            {
                if (result.Main != null)
                    result.Main.Url = "https://smapi.io/";
                if (result.Optional != null)
                    result.Optional.Url = "https://smapi.io/";
            }

            // add extended metadata
            if (includeExtendedMetadata && (wikiEntry != null || record != null))
                result.Metadata = new ModExtendedMetadataModel(wikiEntry, record);

            // add result
            result.Errors = errors.ToArray();
            return result;
        }

        /// <summary>Parse a namespaced mod ID.</summary>
        /// <param name="raw">The raw mod ID to parse.</param>
        /// <param name="vendorKey">The parsed vendor key.</param>
        /// <param name="modID">The parsed mod ID.</param>
        /// <returns>Returns whether the value could be parsed.</returns>
        private bool TryParseModKey(string raw, out string vendorKey, out string modID)
        {
            // split parts
            string[] parts = raw?.Split(':');
            if (parts == null || parts.Length != 2)
            {
                vendorKey = null;
                modID = null;
                return false;
            }

            // parse
            vendorKey = parts[0].Trim();
            modID = parts[1].Trim();
            return true;
        }

        /// <summary>Get whether a <paramref name="current"/> version is newer than an <paramref name="other"/> version.</summary>
        /// <param name="current">The current version.</param>
        /// <param name="other">The other version.</param>
        private bool IsNewer(ISemanticVersion current, ISemanticVersion other)
        {
            return current != null && (other == null || other.IsOlderThan(current));
        }

        /// <summary>Get the mods for which the API should return data.</summary>
        /// <param name="model">The search model.</param>
        /// <param name="apiVersion">The requested API version.</param>
        private IEnumerable<ModSearchEntryModel> GetSearchMods(ModSearchModel model, ISemanticVersion apiVersion)
        {
            if (model == null)
                yield break;

            // yield standard entries
            if (model.Mods != null)
            {
                foreach (ModSearchEntryModel mod in model.Mods)
                    yield return mod;
            }

            // yield mod update keys if backwards compatible
            if (model.ModKeys != null && model.ModKeys.Any() && !apiVersion.IsNewerThan("2.6-beta.17"))
            {
                foreach (string updateKey in model.ModKeys.Distinct())
                    yield return new ModSearchEntryModel(updateKey, new[] { updateKey });
            }
        }

        /// <summary>Get mod data from the wiki compatibility list.</summary>
        private async Task<WikiCompatibilityEntry[]> GetWikiDataAsync()
        {
            ModToolkit toolkit = new ModToolkit();
            return await this.Cache.GetOrCreateAsync($"_wiki", async entry =>
            {
                try
                {
                    WikiCompatibilityEntry[] entries = await toolkit.GetWikiCompatibilityListAsync();
                    entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(this.SuccessCacheMinutes);
                    return entries;
                }
                catch
                {
                    entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(this.ErrorCacheMinutes);
                    return new WikiCompatibilityEntry[0];
                }
            });
        }

        /// <summary>Get the mod info for an update key.</summary>
        /// <param name="updateKey">The namespaced update key.</param>
        private async Task<ModInfoModel> GetInfoForUpdateKeyAsync(string updateKey)
        {
            // parse update key
            if (!this.TryParseModKey(updateKey, out string vendorKey, out string modID))
                return new ModInfoModel($"The update key '{updateKey}' isn't in a valid format. It should contain the site key and mod ID like 'Nexus:541'.");

            // get matching repository
            if (!this.Repositories.TryGetValue(vendorKey, out IModRepository repository))
                return new ModInfoModel($"There's no mod site with key '{vendorKey}'. Expected one of [{string.Join(", ", this.Repositories.Keys)}].");

            // fetch mod info
            return await this.Cache.GetOrCreateAsync($"{repository.VendorKey}:{modID}".ToLower(), async entry =>
            {
                ModInfoModel result = await repository.GetModInfoAsync(modID);
                if (result.Error != null)
                {
                    if (result.Version == null)
                        result.Error = $"The update key '{updateKey}' matches a mod with no version number.";
                    else if (!Regex.IsMatch(result.Version, this.VersionRegex, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
                        result.Error = $"The update key '{updateKey}' matches a mod with invalid semantic version '{result.Version}'.";
                }
                entry.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(result.Error == null ? this.SuccessCacheMinutes : this.ErrorCacheMinutes);
                return result;
            });
        }

        /// <summary>Get the requested API version.</summary>
        private ISemanticVersion GetApiVersion()
        {
            string actualVersion = (string)this.RouteData.Values["version"];
            return new SemanticVersion(actualVersion);
        }
    }
}

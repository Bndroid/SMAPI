# Release notes
## 2.7
* For players:
  * Improved how mod issues are listed in the console and log.
  * Revamped installer. It now...
    * uses a new format that should be more intuitive;
    * lets players on Linux/Mac choose the console color scheme (SMAPI will auto-detect it on Windows);
    * and validates requirements earlier.
  * Fixed custom festival maps always using spring tilesheets.
  * Fixed `player_add` command not recognising return scepter.
  * Fixed `player_add` command showing fish twice.
  * Fixed some SMAPI logs not deleted when starting a new session.

* For modders:
  * Added support for `.json` data files in the content API (including Content Patcher).
  * Added automatic propagation when changing assets through the content API for...
    * child sprites;
    * dialogue;
    * map tilesheets.
  * Added `--mods-path` command-line argument to allow switching between mod folders.
  * All enums are now JSON-serialised by name, since that's more user-friendly. Previously only certain predefined enums were serialised that way. JSON files which already have integer enums will still be parsed fine.
  * Fixed false compatibility error when constructing multidimensional arrays.
  * Fixed `.ToSButton()` methods not being public.
  * Updated compatibility list.

## 2.6
* For players:
  * Updated for Stardew Valley 1.3.
  * Added automatic save backups.
  * Improved update checks:
    * added beta update channel;
    * added update alerts for incompatible mods with an unofficial update on the wiki;
    * added update alerts for optional files on Nexus;
    * added console warning for mods which don't have update checks configured;
    * added more visible prompt in beta channel for SMAPI updates;
    * fixed mod update checks failing if a mod only has prerelease versions on GitHub;
    * fixed Nexus mod update alerts not showing HTTPS links.
  * Improved mod warnings in the console.
  * Improved error when game can't start audio.
  * Improved the Console Commands mod:
    * Added `player_add name`, which adds items to your inventory by name instead of ID.
    * Fixed `world_setseason` not running season-change logic.
    * Fixed `world_setseason` not normalising the season value.
    * Fixed `world_settime` sometimes breaking NPC schedules (e.g. so they stay in bed).
    * Removed the `player_setlevel` and `player_setspeed` commands, which weren't implemented in a useful way. Use a mod like CJB Cheats Menu if you need those.
  * Fixed `SEHException` errors for some players.
  * Fixed performance issues for some players.
  * Fixed default color scheme on Mac or in PowerShell (configurable via `StardewModdingAPI.config.json`).
  * Fixed installer error on Linux/Mac in some cases.
  * Fixed installer not finding some game paths or showing duplicate paths.
  * Fixed installer not removing some SMAPI files.
  * Fixed launch issue for Linux players with some terminals. (Thanks to HanFox and kurumushi!)
  * Fixed abort-retry loop if a mod crashed when intercepting assets during startup.
  * Fixed some mods failing if the player name is blank.
  * Fixed errors when a mod references a missing assembly.
  * Fixed `AssemblyResolutionException` errors in rare cases.
  * Renamed `install.exe` to `install on Windows.exe` to avoid confusion.
  * Updated compatibility list.

* For the web UI:
  * Added option to download SMAPI from Nexus.
  * Added log parser redesign that should be more intuitive.
  * Added log parser option to view raw log.
  * Changed log parser filters to show `DEBUG` messages by default.
  * Fixed design on smaller screens.
  * Fixed log parser issue when content packs have no description.
  * Fixed log parser mangling crossplatform paths in some cases.
  * Fixed `smapi.io/install` not linking to a useful page.

* For modders:
  * Added [input API](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Input) for reading and suppressing keyboard, controller, and mouse input.
  * Added code analysis in the NuGet package to flag common issues as warnings.
  * Replaced `LocationEvents` to support multiplayer:
    * now raised for all locations;
    * now includes added/removed building interiors;
    * each event now provides a list of added/removed values;
    * added buildings-changed event.
  * Added `Context.IsMultiplayer` and `Context.IsMainPlayer` flags.
  * Added `Constants.TargetPlatform` which says whether the game is running on Linux, Mac, or Windows.
  * Added `semanticVersion.IsPrerelease()` method.
  * Added support for launching multiple instances transparently. This removes the former `--log-path` command-line argument.
  * Added support for custom seasonal tilesheets when loading an unpacked `.tbin` map.
  * Added Harmony DLL for internal use by SMAPI. (Mods should still include their own copy for backwards compatibility, and in case it's removed later. SMAPI will always load its own version though.)
  * Added option to suppress update checks for a specific mod in `StardewModdingAPI.config.json`.
  * Added absolute pixels to `ICursorPosition`.
  * Added support for reading/writing `ISemanticVersion` to JSON.
  * Added support for reloading NPC schedules through the content API.
  * Reimplemented the content API so it works more reliably in many edge cases.
  * Reimplemented input suppression to work more consistently in many cases.
  * The order of update keys now affects which URL players see in update alerts.
  * Fixed assets loaded by temporary content managers not being editable by mods.
  * Fixed assets not reloaded consistently when the player switches language.
  * Fixed error if a mod loads a PNG while the game is loading (e.g. custom map tilesheets via `IAssetLoader`).
  * Fixed error if a mod translation file is empty.
  * Fixed input suppression not working consistently for clicks.
  * Fixed console input not saved to the log.
  * Fixed `Context.IsPlayerFree` being false during festivals.
  * Fixed `helper.ModRegistry.GetApi` errors not always mentioning which interface caused the issue.
  * Fixed console commands being invoked asynchronously.
  * Fixed mods able to intercept other mods' assets via the internal asset keys.
  * Fixed mods able to indirectly change other mods' data through shared content caches.
  * Fixed `SemanticVersion` allowing invalid versions in some cases.
  * **Breaking changes** (see [migration guide](https://stardewvalleywiki.com/Modding:Migrate_to_Stardew_Valley_1.3)):
     * Dropped some deprecated APIs.
     * `LocationEvents` have been rewritten.
     * Mods can't intercept chatbox input.
     * Mod IDs should only contain letters, numbers, hyphens, dots, and underscores. That allows their use in many contexts like URLs. This restriction is now enforced. (In regex form: `^[a-zA-Z0-9_.-]+$`.)

* For SMAPI developers:
  * Added more consistent crossplatform handling, including MacOS detection.
  * Added beta update channel.
  * Added optional mod metadata to the web API (including Nexus info, wiki metadata, etc).
  * Added early prototype of SMAPI 3.0 events via `helper.Events`.
  * Added early prototype of mod handler toolkit.
  * Added Harmony for SMAPI's internal use.
  * Added metadata dump option in `StardewModdingAPI.config.json` for troubleshooting some cases.
  * Added more stylish pufferchick on the home page.
  * Rewrote update checks:
    * Moved most logic into the web API.
    * Changed web API to require mod IDs.
    * Changed web API to also fetch metadata from SMAPI's internal mod DB and the wiki.
  * Rewrote world/player state tracking. The new implementation is much more efficient than previous method, uses net field events where available, and lays the groundwork for more advanced events in SMAPI 3.0.
  * Split mod DB out of `StardewModdingAPI.config.json` into its own file.
  * Updated to Mono.Cecil 0.10.

## 2.5.5
* For players:
  * Fixed mod not loaded if it has an optional dependency that's loaded but skipped.
  * Fixed mod update alerts not shown if one mod has an invalid remote version.
  * Fixed SMAPI update alerts linking to the GitHub repository instead of [smapi.io](https://smapi.io).
  * Fixed SMAPI update alerts for draft releases.
  * Fixed error when two content packs use different capitalisation for the same required mod ID.
  * Fixed rare crash if the game duplicates an item.

* For the [log parser][]:
  * Tweaked UI.

## 2.5.4
* For players:
  * Fixed some textures not updated when a mod changes them.
  * Fixed visual bug on Linux/Mac when mods overlay textures.
  * Fixed error when mods remove an asset editor/loader.
  * Fixed minimum game version incorrectly increased in SMAPI 2.5.3.

* For the [log parser][]:
  * Fixed error when log text contains certain tokens.

* For modders:
  * Updated to Json.NET 11.0.2.

* For SMAPI developers:
  * Added support for beta update track to support upcoming Stardew Valley 1.3 beta.

## 2.5.3
* For players:
  * Simplified and improved skipped-mod messages.
  * Fixed rare crash with some combinations of manifest fields and internal mod data.
  * Fixed update checks failing for Nexus Mods due to a change in their API.
  * Fixed update checks failing for some older mods with non-standard versions.
  * Fixed failed update checks being cached for an hour (now cached 5 minutes).
  * Fixed error when a content pack needs a mod that couldn't be loaded.
  * Fixed Linux ["magic number is wrong" errors](https://github.com/mono/mono/issues/6752) by changing default terminal order.
  * Updated compatibility list and added update checks for more mods.

* For the [log parser][]:
  * Fixed incorrect filtering in some cases.
  * Fixed error if mods have duplicate names.
  * Fixed parse bugs if a mod has no author name.

* For SMAPI developers:
  * Internal changes to support the upcoming Stardew Valley 1.3 update.

## 2.5.2
* For modders:
  * Fixed issue where replacing an asset through `asset.AsImage()` or `asset.AsDictionary()` didn't take effect.

* For the [log parser][]:
  * Fixed blank page after uploading a log in some cases.

## 2.5.1
* For players:
  * Fixed event error in rare cases.

## 2.5
* For players:
  * **Added support for [content packs](https://stardewvalleywiki.com/Modding:Content_packs)**.  
    <small>_Content packs are collections of files for a SMAPI mod to load. These can be installed directly under `Mods` like a normal SMAPI mod, get automatic update and compatibility checks, and provide convenient APIs to the mods that read them._</small>
  * Added mod detection for unhandled errors (so most errors now mention which mod caused them).
  * Added install scripts for Linux/Mac (no more manual terminal commands!).
  * Added the missing mod's name and URL to dependency errors.
  * Fixed uninstall script not reporting when done on Linux/Mac.
  * Updated compatibility list and enabled update checks for more mods.

* For modders:
  * Added support for content packs and new APIs to read them.
  * Added support for `ISemanticVersion` in JSON models.
  * Added `SpecialisedEvents.UnvalidatedUpdateTick` event for specialised use cases.
  * Added path normalising to `ReadJsonFile` and `WriteJsonFile` helpers (so no longer need `Path.Combine` with those).
  * Fixed deadlock in rare cases with asset loaders.
  * Fixed unhelpful error when a mod exposes a non-public API.
  * Fixed unhelpful error when a translation file has duplicate keys due to case-insensitivity.
  * Fixed some JSON field names being case-sensitive.

* For the [log parser][]:
  * Added support for SMAPI 2.5 content packs.
  * Reduced download size when viewing a parsed log with repeated errors.
  * Improved parse error handling.
  * Fixed 'log started' field showing incorrect date.

* For SMAPI developers:
  * Overhauled mod DB format to be more concise, reduce the memory footprint, and support versioning/defaulting more fields.
  * Reimplemented log parser with serverside parsing and vue.js on the frontend.

## 2.4
* For players:
  * Fixed visual map glitch in rare cases.
  * Fixed error parsing JSON files which have curly quotes.
  * Fixed error parsing some JSON files generated on another system.
  * Fixed error parsing some JSON files after mods reload core assemblies, which is no longer allowed.
  * Fixed intermittent errors (e.g. 'collection has been modified') with some mods when loading a save.
  * Fixed compatibility with Linux Terminator terminal.

* For the [log parser][]:
  * Fixed error parsing logs with zero installed mods.

* For modders:
  * Added `SaveEvents.BeforeCreate` and `AfterCreate` events.
  * Added `SButton` `IsActionButton()` and `IsUseToolButton()` extensions.
  * Improved JSON parse errors to provide more useful info for troubleshooting.
  * Fixed events being raised while the game is loading a save file.
  * Fixed input events not recognising controller input as an action or use-tool button.
  * Fixed input events setting the same `IsActionButton` and `IsUseToolButton` values for all buttons pressed in an update tick.
  * Fixed semantic versions ignoring `-0` as a prerelease tag.
  * Updated Json.NET to 11.0.1-beta3 (needed to avoid a parser edge case).

* For SMAPI developers:
  * Overhauled input handling to support future input events.

## 2.3
* For players:
  * Added a user-friendly [download page](https://smapi.io).
  * Improved cryptic libgdiplus errors on Mac when Mono isn't installed.
  * Fixed mod UIs hidden when menu backgrounds are enabled.

* For modders:
  * **Added mod-provided APIs** to allow simple integrations between mods, even without direct assembly references.
  * Added `GameEvents.FirstUpdateTick` event (called once after all mods are initialised).
  * Added `IsSuppressed` to input events so mods can optionally avoid handling keys another mod has already handled.
  * Added trace message for mods with no update keys.
  * Adjusted reflection API to match actual usage (e.g. renamed `GetPrivate*` to `Get*`), and deprecated previous methods.
  * Fixed `GraphicsEvents.OnPostRenderEvent` not being raised in some specialised cases.
  * Fixed reflection API error for properties missing a `get` and `set`.
  * Fixed issue where a mod could change the cursor position reported to other mods.
  * Updated compatibility list.

* For the [log parser][]:
  * Fixed broken favicon.

## 2.2
* For players:
  * Fixed error when a mod loads custom assets on Linux/Mac.
  * Fixed error when checking for updates on Linux/Mac due to API HTTPS redirect.
  * Fixed error when Mac adds an `mcs` symlink to the installer package.
  * Fixed `player_add` command not handling tool upgrade levels.
  * Improved error when a mod has an invalid `EntryDLL` filename format.
  * Updated compatibility list.

* For the [log parser][]:
  * Logs no longer expire after a week.
  * Fixed error when uploading very large logs.
  * Slightly improved the UI.

* For modders:
  * Added `helper.Content.NormaliseAssetName` method.
  * Added `SDate.DaysSinceStart` property.
  * Fixed input events' `e.SuppressButton(button)` method ignoring specified button.
  * Fixed input events' `e.SuppressButton()` method not working with mouse buttons.

## 2.1
* For players:
  * Added a [log parser][] site.
  * Added better Steam instructions to the SMAPI installer.
  * Renamed the bundled _TrainerMod_ to _ConsoleCommands_ to make its purpose clearer.
  * Removed the game's test messages from the console log.
  * Improved update-check errors when playing offline.
  * Fixed compatibility check for players with Stardew Valley 1.08.
  * Fixed `player_setlevel` command not setting XP too.

* For modders:
  * The reflection API now works with public code to simplify mod integrations.
  * The content API now lets you invalidated multiple assets at once.
  * The `InputEvents` have been improved:
    * Added `e.IsActionButton` and `e.IsUseToolButton`.
    * Added `ToSButton()` extension for the game's `Game1.options` button type.
    * Deprecated `e.IsClick`, which is limited and unclear. Use `IsActionButton` or `IsUseToolButton` instead.
    * Fixed `e.SuppressButton()` not correctly suppressing keyboard buttons.
    * Fixed `e.IsClick` (now `e.IsActionButton`) ignoring custom key bindings.
  * `SemanticVersion` can now be constructed from a `System.Version`.
  * Fixed reflection API blocking access to some non-SMAPI members.
  * Fixed content API allowing absolute paths as asset keys.
  * Fixed content API failing to load custom map tilesheets that aren't preloaded.
  * Fixed content API incorrectly detecting duplicate loaders when a mod implements `IAssetLoader` directly.

* For SMAPI developers:
  * Added the installer version and platform to the installer window title to simplify troubleshooting.

## 2.0
### Release highlights
* **Mod update checks**  
  SMAPI now checks if your mods have updates available, and will alert you in the console with a convenient link to the
  mod page. This works with mods from the Chucklefish mod site, GitHub, or Nexus Mods. SMAPI 2.0 launches with
  update-check support for over 250 existing mods, and more will be added as modders enable the feature.

* **Mod stability warnings**  
  SMAPI now detects when a mod contains code which can destabilise your game or corrupt your save, and shows a warning
  in the console.

* **Simpler console**  
   The console is now simpler and easier to read, some commands have been streamlined, and the colors now adjust to fit
   your terminal background color.

* **New features for modders**  
  SMAPI 2.0 adds several features to enable new kinds of mods (see
  [API documentation](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs)).

  The **content API** lets you edit, inject, and reload XNB data loaded by the game at any time. This lets SMAPI mods do
  anything previously only possible with XNB mods, and enables new mod scenarios not possible with XNB mods (e.g.
  seasonal textures, NPC clothing that depend on the weather or location, etc).

  The **input events** unify controller + keyboard + mouse input into one event and constant for easy handling, and add
  metadata like the cursor position and grab tile to support click handling. They also let you prevent the game from
  receiving input, to enable new scenarios like action highjacking and UI overlays.

  The mod manifest has a few changes too:
  * The **`UpdateKeys` field** lets you specify your Chucklefish, GitHub, or Nexus mod IDs. SMAPI will automatically
    check for newer versions and notify the player.
  * The **version field** is now a semantic string like `"1.0-alpha"`. (Mods which still use the version structure will
    still work fine.)
  * The **dependencies field** now lets you add optional dependencies which should be loaded first if available.

  Finally, the `SDate` utility now has a `DayOfWeek` field for more convenient date calculations, and `ISemanticVersion`
  now implements `IEquatable<ISemanticVersion>`.

* **Goodbye deprecated code**  
  SMAPI 2.0 removes all deprecated code to unshackle future development. That includes...
  * removed all code marked obsolete;
  * removed TrainerMod's `save` and `load` commands;
  * removed support for mods with no `Name`, `Version`, or `UniqueID` in their manifest;
  * removed support for multiple mods having the same `UniqueID` value;
  * removed access to SMAPI internals through the reflection helper.

* **Command-line install**
  For power users and mod managers, the SMAPI installer can now be scripted using command-line arguments
  (see [technical docs](technical-docs.md#command-line-arguments)).

### Change log
For players:
* SMAPI now alerts you when mods have new versions available.
* SMAPI now warns you about mods which may impact game stability or compatibility.
* The console is now simpler and easier to read, and adjusts its colors to fit your terminal background color.
* Renamed installer folder to avoid confusion.
* Updated compatibility list.
* Fixed update check errors on Linux/Mac.
* Fixed collection-changed errors during startup for some players.

For mod developers:
* Added support for editing, injecting, and reloading XNB data loaded by the game at any time.
* Added support for automatic mod update checks.
* Added unified input events.
* Added support for suppressing input.
* Added support for optional dependencies.
* Added support for specifying the mod version as a string (like `"1.0-alpha"`) in `manifest.json`.
* Added day of week to `SDate` instances.
* Added `IEquatable<ISemanticVersion>` to `ISemanticVersion`.
* Updated Json.NET from 8.0.3 to 10.0.3.
* Removed the TrainerMod's `save` and `load` commands.
* Removed all deprecated code.
* Removed support for mods with no `Name`, `Version`, or `UniqueID` in their manifest.
* Removed support for mods with a non-unique `UniqueID` value in their manifest.
* Removed access to SMAPI internals through the reflection helper, to discourage fragile mods.
* Fixed `SDate.Now()` crashing when called during the new-game intro.
* Fixed `TimeEvents.AfterDayStarted` being raised during the new-game intro.
* Fixed SMAPI allowing map tilesheets with absolute or directory-climbing paths. These are now rejected even if the path exists, to avoid problems when players install the mod.

For power users:
* Added command-line arguments to the SMAPI installer so it can be scripted.

For SMAPI developers:
* Significantly refactored SMAPI to support changes in 2.0 and upcoming releases.
* Overhauled `StardewModdingAPI.config.json` format to support mod data like update keys.
* Removed SMAPI 1._x_ compatibility mode.

## 1.15.4
For players:
* Fixed errors when loading some custom maps on Linux/Mac or using XNB Loader.
* Fixed errors in rare cases when a mod calculates an in-game date.

For modders:
* Added UTC timestamp to log file.

For SMAPI developers:
* Internal changes to support the upcoming SMAPI 2.0 release.

## 1.15.3
For players:
* Fixed mods being wrongly marked as duplicate in some cases.

## 1.15.2
For players:
* Improved errors when a mod DLL can't be loaded.
* Improved errors when using very old versions of Stardew Valley.
* Updated compatibility list.

For mod developers:
* Added `Context.CanPlayerMove` property for mod convenience.
* Added content helper properties for the game's current language.
* Fixed `Context.IsPlayerFree` being false if the player is performing an action.
* Fixed `GraphicsEvents.Resize` being raised before the game updates its window data.
* Fixed `SemanticVersion` not being deserialisable through Json.NET.
* Fixed terminal not launching on Xfce Linux.

For SMAPI developers:
* Internal changes to support the upcoming SMAPI 2.0 release.

## 1.15.1
For players:
* Fixed controller mod input broken in 1.15.
* Fixed TrainerMod packaging unneeded files.

For modders:
* Fixed mod registry lookups by unique ID not being case-insensitive.

## 1.15
For players:
* Cleaned up SMAPI console a bit.
* Revamped TrainerMod's item commands:
  * `player_add` is a new command to add any item to your inventory (including tools, weapons, equipment, craftables, wallpaper, etc). This replaces the former `player_additem`, `player_addring`, and `player_addweapon`.
  * `list_items` now shows all items in the game. You can search by item type like `list_items weapon`, or search by item name like `list_items galaxy sword`.
  * `list_items` now also matches translated item names when playing in another language.
  * `list_item_types` is a new command to see a list of item types.
* Fixed unhelpful error when a `config.json` is invalid.
* Fixed rare crash when window loses focus for a few players (further to fix in 1.14).
* Fixed invalid `ObjectInformation.xnb` causing a flood of warnings; SMAPI now shows one error instead.
* Updated mod compatibility list.

For modders:
* Added `SDate` utility for in-game date calculations (see [API reference](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Utilities#Dates)).
* Added support for minimum dependency versions in `manifest.json` (see [API reference](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Manifest)).
* Added more useful logging when loading mods.
* Added a `ModID` property to all mod helpers for extension methods.
* Changed `manifest.MinimumApiVersion` from string to `ISemanticVersion`. This shouldn't affect mods unless they referenced that field in code.
* Fixed `SemanticVersion` parsing some invalid versions into close approximations (like `1.apple` &rarr; `1.0-apple`).
* Fixed `SemanticVersion` not treating hyphens as separators when comparing prerelease tags.  
  <small>_(While that was technically correct, it leads to unintuitive behaviour like sorting `-alpha-2` _after_ `-alpha-10`, even though `-alpha.2` sorts before `-alpha.10`.)_</small>
* Fixed corrupted state exceptions not being logged by SMAPI.
* Increased all deprecations to _pending removal_.

For SMAPI developers:
* Added SMAPI 2.0 compile mode, for testing how mods will work with SMAPI 2.0.
* Added prototype SMAPI 2.0 feature to override XNB files (not enabled for mods yet).
* Added prototype SMAPI 2.0 support for version strings in `manifest.json` (not recommended for mods yet).
* Compiling SMAPI now uses your `~/stardewvalley.targets` file if present.

## 1.14
See [log](https://github.com/Pathoschild/SMAPI/compare/1.13...1.14).

For players:
* SMAPI now shows friendly errors when...
  * it can't detect the game;
  * a mod dependency is missing (if it's listed in the mod manifest);
  * you have Stardew Valley 1.11 or earlier (which aren't compatible);
  * you run `install.exe` from within the downloaded zip file.
* Fixed "unknown mod" deprecation warnings by improving how SMAPI detects the mod using the event.
* Fixed `libgdiplus.dylib` errors for some players on Mac.
* Fixed rare crash when window loses focus for a few players.
* Bumped minimum game version to 1.2.30.
* Updated mod compatibility list.

For modders:
* You can now add dependencies to `manifest.json` (see [API reference](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Manifest)).
* You can now translate your mod (see [API reference](https://stardewvalleywiki.com/Modding:Modder_Guide/APIs/Translation)).
* You can now load unpacked `.tbin` files from your mod folder through the content API.  
* SMAPI now automatically fixes tilesheet references for maps loaded from the mod folder.  
  <small>_When loading a map from the mod folder, SMAPI will automatically use tilesheets relative to the map file if they exists. Otherwise it will default to tilesheets in the game content._</small>
* Added `Context.IsPlayerFree` for mods that need to check if the player can act (i.e. save is loaded, no menu is displayed, no cutscene is in progress, etc).
* Added `Context.IsInDrawLoop` for specialised mods.
* Fixed `smapi-crash.txt` being copied from the default log even if a different path is specified with `--log-path`.
* Fixed the content API not matching XNB filenames with two dots (like `a.b.xnb`) if you don't specify the `.xnb` extension.
* Fixed `debug` command output not printed to console.
* Deprecated `TimeEvents.DayOfMonthChanged`, `SeasonOfYearChanged`, and `YearOfGameChanged`. These don't do what most modders think they do and aren't very reliable, since they depend on the SMAPI/game lifecycle which can change. You should use `TimeEvents.AfterDayStarted` or `SaveEvents.BeforeSave` instead.

## 1.13.1
For players:
* Fixed errors when loading a mod with no name or version.
* Fixed mods with no manifest `Name` field having no name (SMAPI will now shows their filename).

## 1.13
See [log](https://github.com/Pathoschild/SMAPI/compare/1.12...1.13).

For players:
* SMAPI now recovers better from mod draw errors and detects when the error is irrecoverable.
* SMAPI now recovers automatically from errors in the game loop when possible.
* SMAPI now remembers if your game crashed and offers help next time you launch it.
* Fixed installer sometimes finding redundant game paths.
* Fixed save events not being raised after the first day on Linux/Mac.
* Fixed error on Linux/Mac when a mod loads a PNG immediately after the save is loaded.
* Updated mod compatibility list for Stardew Valley 1.2.

For mod developers:
* Added a `Context.IsWorldReady` flag for mods to use.  
  <small>_This indicates whether a save is loaded and the world is finished initialising, which starts at the same point that `SaveEvents.AfterLoad` and `TimeEvents.AfterDayStarted` are raised. This is mainly useful for events which can be raised before the world is loaded (like update tick)._</small>
* Added a `debug` console command which lets you run the game's debug commands (e.g. `debug warp FarmHouse 1 1` warps you to the farmhouse).
* Added basic context info to logs to simplify troubleshooting.
* Added a `Mod.Dispose` method which can be overriden to clean up before exit. This method isn't guaranteed to be called on every exit.
* Deprecated mods that don't have a `Name`, `Version`, or `UniqueID` in their manifest. These will be required in SMAPI 2.0.
* Deprecated `GameEvents.GameLoaded` and `GameEvents.FirstUpdateTick`. You can move any affected code into your mod's `Entry` method.
* Fixed maps not recognising custom tilesheets added through the SMAPI content API.
* Internal refactoring for upcoming features.

## 1.12
See [log](https://github.com/Pathoschild/SMAPI/compare/1.11...1.12).

For players:
* The installer now lets you choose the install path if you have multiple copies of the game, instead of using the first path found.
* Fixed mod draw errors breaking the game.
* Fixed mods on Linux/Mac no longer working after the game saves.
* Fixed `libgdiplus.dylib` errors on Mac when mods read PNG files.
* Adopted pufferchick.

For mod developers:
* Unknown mod manifest fields are now stored in `IManifest::ExtraFields`.
* The content API now defaults to `ContentSource.ModFolder`.
* Fixed content API error when loading a PNG during early game init (e.g. in mod's `Entry`).
* Fixed content API error when loading an XNB from the mod folder on Mac.

## 1.11
See [log](https://github.com/Pathoschild/SMAPI/compare/1.10...1.11).

For players:
* SMAPI now detects issues in `ObjectInformation.xnb` files caused by outdated XNB mods.
* Errors when loading a save are now shown in the SMAPI console.
* Improved console logging performance.
* Fixed errors during game update causing the game to hang.
* Fixed errors due to mod events triggering during game save in Stardew Valley 1.2.

For mod developers:
* Added a content API which loads custom textures/maps/data from the mod's folder (`.xnb` or `.png` format) or game content.
* `Console.Out` messages are now written to the log file.
* `Monitor.ExitGameImmediately` now aborts SMAPI initialisation and events more quickly.
* Fixed value-changed events being raised when the player loads a save due to values being initialised.

## 1.10
See [log](https://github.com/Pathoschild/SMAPI/compare/1.9...1.10).

For players:
* Updated to Stardew Valley 1.2.
* Added logic to rewrite many mods for compatibility with game updates, though some mods may still need an update.
* Fixed `SEHException` errors affecting some players.
* Fixed issue where SMAPI didn't unlock some files on exit.
* Fixed rare issue where the installer would crash trying to delete a bundled mod from `%appdata%`.
* Improved TrainerMod commands:
  * Added `world_setyear` to change the current year.
  * Replaced `player_addmelee` with `player_addweapon` with support for non-melee weapons.

For mod developers:
* Mods are now initialised after the `Initialize`/`LoadContent` phase, which means the `GameEvents.Initialize` and `GameEvents.LoadContent` events are deprecated. You can move any logic in those methods to your mod's `Entry` method.
* Added `IsBetween` and string overloads to the `ISemanticVersion` methods.
* Fixed mouse-changed event never updating prior mouse position.
* Fixed `monitor.ExitGameImmediately` not working correctly.
* Fixed `Constants.SaveFolderName` not set for a new game until the save is created.

## 1.9
See [log](https://github.com/Pathoschild/SMAPI/compare/1.8...1.9).

For players:
* SMAPI now detects incompatible mods and disables them before they cause problems.
* SMAPI now allows mods nested into an otherwise empty parent folder (like `Mods\ModName-1.0\ModName\manifest.json`), since that's a common default behaviour when unpacking mods.
* The installer now detects if you need to update .NET Framework before installing SMAPI.
* The installer now detects if you need to run the game at least once (to let it perform first-time setup) before installing SMAPI.
* The installer on Linux now finds games installed to `~/.steam/steam/steamapps/common/Stardew Valley` too.
* The installer now removes old SMAPI logs to prevent confusion.
* The console now has simpler error messages.
* The console now has improved command handling & feedback.
* The console no longer shows the game's debug output (unless you use a _SMAPI for developers_ build).
* Fixed the game-needs-an-update error not pausing before exit.
* Fixed installer errors for some players when deleting files.
* Fixed installer not ignoring potential game folders that don't contain a Stardew Valley exe.
* Fixed installer not recognising Linux/Mac paths starting with `~/` or containing an escaped space.
* Fixed TrainerMod letting you add invalid items which may crash the game.
* Fixed TrainerMod's `world_downminelevel` command not working.
* Fixed rare issue where mod dependencies would override SMAPI dependencies and cause unpredictable bugs.
* Fixed errors in mods' console command handlers crashing the game.

For mod developers:
* Added a simpler API for console commands (see `helper.ConsoleCommands`).
* Added `TimeEvents.AfterDayStarted` event triggered when a day starts. This happens no matter how the day started (including new game, loaded save, or player went to bed).
* Added `ContentEvents.AfterLocaleChanged` event triggered when the player changes the content language (for the upcoming Stardew Valley 1.2).
* Added `SaveEvents.AfterReturnToTitle` event triggered when the player returns to the title screen (for the upcoming Stardew Valley 1.2).
* Added `helper.Reflection.GetPrivateProperty` method.
* Added a `--log-path` argument to specify the SMAPI log path during testing.
* SMAPI now writes XNA input enums (`Buttons` and `Keys`) to JSON as strings automatically, so mods no longer need to add a `StringEnumConverter` themselves for those.
* The SMAPI log now has a simpler filename.
* The SMAPI log now shows the OS caption (like "Windows 10") instead of its internal version when available.
* The SMAPI log now always uses `\r\n` line endings to simplify crossplatform viewing.
* Fixed `SaveEvents.AfterLoad` being raised during the new-game intro before the player is initialised.
* Fixed SMAPI not recognising `Mod` instances that don't subclass `Mod` directly.
* Several obsolete APIs have been removed (see [deprecation guide](http://canimod.com/guides/updating-a-smapi-mod)),
  and all _notice_-level deprecations have been increased to _info_.
* Removed the experimental `IConfigFile`.

For SMAPI developers:
* Added support for debugging SMAPI on Linux/Mac if supported by the editor.

## 1.8
See [log](https://github.com/Pathoschild/SMAPI/compare/1.7...1.8).

For players:
* Mods no longer generate `.cache` subfolders.
* Fixed multiple issues where mods failed during assembly loading.
* Tweaked install package to reduce confusion.

For mod developers:
* The `SemanticVersion` constructor now accepts a string version.
* Increased deprecation level for `Extensions` to _pending removal_.
* **Warning:** `Assembly.GetExecutingAssembly().Location` will no longer reliably
  return a valid path, because mod assemblies are loaded from memory when rewritten for
  compatibility. This approach has been discouraged since SMAPI 1.3; use `helper.DirectoryPath`
  instead.

For SMAPI developers:
* Rewrote assembly loading from the ground up. The new implementation...
  * is much simpler;
  * eliminates the `.cache` folders by loading rewritten assemblies from memory;
  * ensures DLLs are loaded in leaf-to-root order (i.e. dependencies first);
  * improves dependent assembly resolution;
  * no longer loads DLLs if they're not referenced;
  * reduces log verbosity.

## 1.7
See [log](https://github.com/Pathoschild/SMAPI/compare/1.6...1.7).

For players:
* The console now shows the folder path where mods should be added.
* The console now shows deprecation warnings after the list of loaded mods (instead of intermingled).

For mod developers:
* Added a mod registry which provides metadata about loaded mods.
* The `Entry(…)` method is now deferred until all mods are loaded.
* Fixed `SaveEvents.BeforeSave` and `.AfterSave` not triggering on days when the player shipped something.
* Fixed `PlayerEvents.LoadedGame` and `SaveEvents.AfterLoad` being fired before the world finishes initialising.
* Fixed some `LocationEvents`, `PlayerEvents`, and `TimeEvents` being fired during game startup.
* Increased deprecation levels for `SObject`, `LogWriter` (not `Log`), and `Mod.Entry(ModHelper)` (not `Mod.Entry(IModHelper)`) to _pending removal_. Increased deprecation levels for `Mod.PerSaveConfigFolder`, `Mod.PerSaveConfigPath`, and `Version.VersionString` to _info_.

## 1.6
See [log](https://github.com/Pathoschild/SMAPI/compare/1.5...1.6).

For players:
* Added console commands to open the game/data folders.
* Updated list of incompatible mods.
* Fixed `config.json` values being duplicated in some cases.
* Fixed some Linux users not being able to launch SMAPI from Steam.
* Fixed the installer not finding custom install paths on 32-bit Windows.
* Fixed error when loading a mod which was released with a `.cache` folder for a different platform.
* Fixed error when the console doesn't support colour.
* Fixed error when a mod reads a custom JSON file from a directory that doesn't exist.

For mod developers:
* Added three events: `SaveEvents.BeforeSave`, `SaveEvents.AfterSave`, and `SaveEvents.AfterLoad`.
* Deprecated three events:
  * `TimeEvents.OnNewDay` is unreliable; use `TimeEvents.DayOfMonthChanged` or `SaveEvents` instead.
  * `PlayerEvents.LoadedGame` is replaced by `SaveEvents.AfterLoad`.
  * `PlayerEvents.FarmerChanged` serves no purpose.

For SMAPI developers:
  * Added support for specifying a lower bound in mod incompatibility data.
  * Added support for custom incompatible-mod error text.
  * Fixed issue where `TrainerMod` used older logic to detect the game path.

## 1.5
See [log](https://github.com/Pathoschild/SMAPI/compare/1.4...1.5).

For players:
  * Added an option to disable update checks.
  * SMAPI will now show a friendly error with update links when you try to use a known incompatible mod version.
  * Fixed an error when a mod uses the new reflection API on a missing field or method.
  * Fixed an issue where mods weren't notified of a menu change if it changed while SMAPI was still notifying mods of the previous change.

For developers:
  * Deprecated `Version` in favour of `SemanticVersion`.  
    _This new implementation is [semver 2.0](http://semver.org/)-compliant, introduces `NewerThan(version)` and `OlderThan(version)` convenience methods, adds support for parsing a version string into a `SemanticVersion`, and fixes various bugs with the former implementation. This also replaces `Manifest` with `IManifest`._
  * Increased deprecation levels for `SObject`, `Extensions`, `LogWriter` (not `Log`), `SPlayer`, and `Mod.Entry(ModHelper)` (not `Mod.Entry(IModHelper)`).

## 1.4
See [log](https://github.com/Pathoschild/SMAPI/compare/1.3...1.4).

For players:
  * SMAPI will now prevent mods from crashing your game with menu errors.
  * The installer will now automatically detect most custom install paths.
  * The installer will now automatically clean up old SMAPI files.
  * Each mod now has its own `.cache` folder, so removing the mod won't leave orphaned cache files behind.
  * Improved installer wording to reduce confusion.
  * Fixed the installer not removing TrainerMod from appdata if it's already in the game mods directory.
  * Fixed the installer not moving mods out of appdata if the game isn't installed on the same Windows partition.
  * Fixed the SMAPI console not being shown on Linux and Mac.

For developers:
  * Added a reflection API (via `helper.Reflection`) that simplifies robust access to the game's private fields and methods.
  * Added a searchable `list_items` console command to replace the `out_items`, `out_melee`, and `out_rings` commands.
  * Added `TypeLoadException` details when intercepted by SMAPI.
  * Fixed an issue where you couldn't debug into an assembly because it was copied into the `.cache` directory. That will now only happen if necessary.

## 1.3
See [log](https://github.com/Pathoschild/SMAPI/compare/1.2...1.3).

For players:
  * You can now run most mods on any platform (e.g. run Windows mods on Linux/Mac).
  * Fixed the normal uninstaller not removing files added by the 'SMAPI for developers' installer.

## 1.2
See [log](https://github.com/Pathoschild/SMAPI/compare/1.1.1...1.2).

For players:
  * Fixed compatibility with some older mods.
  * Fixed mod errors in most event handlers crashing the game.
  * Fixed mod errors in some event handlers preventing other mods from receiving the same event.
  * Fixed game crashing on startup with an audio error for some players.

For developers:
  * Improved logging to show `ReflectionTypeLoadException` details when it's caught by SMAPI.

## 1.1
See [log](https://github.com/Pathoschild/SMAPI/compare/1.0...1.1.1).

For players:
  * Fixed console exiting immediately when some exceptions occur.
  * Fixed an error in 1.0 when mod uses `config.json` but the file doesn't exist.
  * Fixed critical errors being saved to a separate log file.
  * Fixed compatibility with some older mods.<sup>1.1.1</sup>
  * Fixed race condition where some mods would sometimes crash because the game wasn't ready yet.<sup>1.1.1</sup>

For developers:
  * Added new logging interface:
    * easier to use;
    * supports trace logs (written to the log file, but hidden in the console by default);
    * messages are now listed in order;
    * messages now show which mod logged them;
    * more consistent and intuitive console color scheme.
  * Added optional `MinimumApiVersion` to `manifest.json`.
  * Added emergency interrupt feature for dangerous mods.
  * Fixed deprecation warnings being repeated if the mod can't be identified.<sup>1.1.1</sup>

## 1.0
See [log](https://github.com/Pathoschild/SMAPI/compare/0.40.1.1-3...1.0).

For players:
  * Added support for Linux and Mac.
  * Added installer to automate adding & removing SMAPI.
  * Added background update check on launch.
  * Fixed missing `steam_appid.txt` file.
  * Fixed some mod UIs disappearing at a non-default zoom level for some users.
  * Removed undocumented support for mods in AppData folder **(breaking change)**.
  * Removed `F2` debug mode.

For mod developers:
  * Added deprecation warnings.
  * Added OS version to log.
  * Added zoom-adjusted mouse position to mouse-changed event arguments.
  * Added SMAPI code documentation.
  * Switched to [semantic versioning](http://semver.org).
  * Fixed mod versions not shown correctly in the log.
  * Fixed misspelled field in `manifest.json` schema.
  * Fixed some events getting wrong data.
  * Simplified log output.

For SMAPI developers:
  * Simplified compiling from source.
  * Formalised release process and added automated build packaging.
  * Removed obsolete and unfinished code.
  * Internal cleanup & refactoring.

## 0.x
* 0.40.1.1 (2016-09-30, [log](https://github.com/Pathoschild/SMAPI/compare/0.40.0...0.40.1.1-3))
  * Added support for Stardew Valley 1.1.

* 0.40.0 (2016-04-05, [log](https://github.com/Pathoschild/SMAPI/compare/0.39.7...0.40.0))
  * Fixed an error that ocurred during minigames.

* 0.39.7 (2016-04-04, [log](https://github.com/Pathoschild/SMAPI/compare/0.39.6...0.39.7))
  * Added 'no check' graphics events that are triggered regardless of game's if checks.

* 0.39.6 (2016-04-01, [log](https://github.com/Pathoschild/SMAPI/compare/0.39.5...0.39.6))
  * Added game & SMAPI versions to log.
  * Fixed conflict in graphics tick events.
  * Bug fixes.

* 0.39.5 (2016-03-30, [log](https://github.com/Pathoschild/SMAPI/compare/0.39.4...0.39.5))
* 0.39.4 (2016-03-29, [log](https://github.com/Pathoschild/SMAPI/compare/0.39.3...0.39.4))
* 0.39.3 (2016-03-28, [log](https://github.com/Pathoschild/SMAPI/compare/0.39.2...0.39.3))
* 0.39.2 (2016-03-23, [log](https://github.com/Pathoschild/SMAPI/compare/0.39.1...0.39.2))
* 0.39.1 (2016-03-23, [log](https://github.com/Pathoschild/SMAPI/compare/0.38.8...0.39.1))
* 0.38.8 (2016-03-23, [log](https://github.com/Pathoschild/SMAPI/compare/0.38.7...0.38.8))
* 0.38.7 (2016-03-23, [log](https://github.com/Pathoschild/SMAPI/compare/0.38.6...0.38.7))
* 0.38.6 (2016-03-22, [log](https://github.com/Pathoschild/SMAPI/compare/0.38.5...0.38.6))
* 0.38.5 (2016-03-22, [log](https://github.com/Pathoschild/SMAPI/compare/0.38.4...0.38.5))
* 0.38.4 (2016-03-21, [log](https://github.com/Pathoschild/SMAPI/compare/0.38.3...0.38.4))
* 0.38.3 (2016-03-21, [log](https://github.com/Pathoschild/SMAPI/compare/0.38.2...0.38.3))
* 0.38.2 (2016-03-21, [log](https://github.com/Pathoschild/SMAPI/compare/0.38.0...0.38.2))
* 0.38.0 (2016-03-20, [log](https://github.com/Pathoschild/SMAPI/compare/0.38.1...0.38.0))
* 0.38.1 (2016-03-20, [log](https://github.com/Pathoschild/SMAPI/compare/0.37.3...0.38.1))
* 0.37.3 (2016-03-08, [log](https://github.com/Pathoschild/SMAPI/compare/0.37.2...0.37.3))
* 0.37.2 (2016-03-07, [log](https://github.com/Pathoschild/SMAPI/compare/0.37.1...0.37.2))
* 0.37.1 (2016-03-06, [log](https://github.com/Pathoschild/SMAPI/compare/0.36...0.37.1))
* 0.36 (2016-03-04, [log](https://github.com/Pathoschild/SMAPI/compare/0.37...0.36))
* 0.37 (2016-03-04, [log](https://github.com/Pathoschild/SMAPI/compare/0.35...0.37))
* 0.35 (2016-03-02, [log](https://github.com/Pathoschild/SMAPI/compare/0.34...0.35))
* 0.34 (2016-03-02, [log](https://github.com/Pathoschild/SMAPI/compare/0.33...0.34))
* 0.33 (2016-03-02, [log](https://github.com/Pathoschild/SMAPI/compare/0.32...0.33))
* 0.32 (2016-03-02, [log](https://github.com/Pathoschild/SMAPI/compare/0.31...0.32))
* 0.31 (2016-03-02, [log](https://github.com/Pathoschild/SMAPI/compare/0.3...0.31))
* 0.3 (2016-03-01, [log](https://github.com/Pathoschild/SMAPI/compare/Alpha0.2...0.3))
* 0.2 (2016-02-29, [log](https://github.com/Pathoschild/SMAPI/compare/Alpha0.1...Alpha0.2)
* 0.1 (2016-02-28)

[log parser]: https://log.smapi.io

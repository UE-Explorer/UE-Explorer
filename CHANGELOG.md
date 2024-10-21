#

## 1.4.3

* Hotfix for #66
* Re-wrote the initialization of tree nodes to use a background async task (this improves the UI performance drastically)

* Updated to UELib 1.7.0 from 1.6.1

### UELib 1.7.0

#### [1.7.0](https://github.com/EliotVU/Unreal-Library/releases/tag/1.7.0)

* Support for Mass Effect: Legendary Edition *(not all releases are compatible)*
* Support for Men of Valor
* Support for Tom Clancy's Splinter Cell: Double Agent (Offline Mode)
* Support for Stargate SG-1: The Alliance
* Improved support for Frontlines: Fuel of War

## 1.4.2

* Various fixes
* Added a dropdown to tabs which lets you open the package in the "File Explorer"
* Added a tooltip to tabs to display its full file path.

* Updated to UELib 1.6.1 from 1.5.0

### UELib 1.6.1 - 1.5.1 Changelog

#### [1.6.1](https://github.com/EliotVU/Unreal-Library/releases/tag/1.6.1)

* Added a comment to enum tags to display its value.
* Fixed the decompilation output of an element access expression (in a T3D context) for UE1 based games: Changed `Element[0]=Value` to `Element(0)=Value`

#### [1.6.0](https://github.com/EliotVU/Unreal-Library/releases/tag/1.6.0)

* Support for Tom Clancy's EndWar
* Support for Gigantic: Rampage Edition (thanks to @HyenaCoding)
* Support for Borderlands: Game of the Year Enhanced; and fixed regression of Borderlands and Battleborn.
* Improved support for Duke Nukem Forever (thanks to @DaZombieKiller)
* Improved support for Rocket League
* Fixed regression [Batman series support](https://github.com/UE-Explorer/UE-Explorer/issues/63)

* Fixed fallback for deprecated ClassName so that "UE Explorer" can pickup content again.
* Fixed #36; various T3D archetype fixes.
* Fixed T3D syntax ouput from "object end" to "end object"

#### [1.5.1](https://github.com/EliotVU/Unreal-Library/releases/tag/1.5.1)

* Fixed regression #74; The deprecated `UnrealConfig.CookedPlatform` field was ignored, which is still relevant for legacy-code, thanks to @Hox8
* Updated auto-detected builds for the Infinity Blade series

## 1.4.1

* Fixed #30 and #35; suppresses signature warning for Hawken and Killing Floor packages.
* Fixed regression #49; thanks to @Un-Drew
* Added a history navigation dropdown button #47
* QoL updates to the "Hex Viewer", such as:
  * Copy Cell (value, size, offset; either in hex or decimal format)
  * Edit Struct (allows one to edit names and objects by selecting an object using a dialog)
  * Remove Struct (removes an applied struct)
* Updated to UELib 1.5.0 from 1.3.1

### UELib 1.5.0 - 1.4.0 Changelog

#### [1.5.0](https://github.com/EliotVU/Unreal-Library/releases/tag/1.5.0)

* 1ef135d Improved support for A Hat in Time (UE3), contributed by @Un-Drew

#### [1.4.0](https://github.com/EliotVU/Unreal-Library/releases/tag/1.4.0)

Notable changes that affect UnrealScript output:

* Improved decompilation output of string and decimal literals.
* 5141285c Improved decompilation of delegate assignments (in a T3D context)
* 6d889c87 Added decompilation of optional parameter assignments e.g. `function MyFunction(option bool A = true);`.
* e55cfce0 Fixed decompilation with arrays of bools

Notable changes that affect support of games:

General deserialization fixes that affect all of UE1, UE2, and UE3 builds, as well as more specifically:

* 13460cca Support for Battleborn
* 4aff61fa Support for Duke Nukem Forever (2011)
* bce38c4f Support for Tom Clancy's Splinter Cell
* 809edaad Support for Harry Potter (UE1) data class {USound}
* b3e1489d Support for Devastation (UE2, 2003)
* 4780771a Support for Clive Barker's Undying (UE1) data classes {UClass, UTextBuffer, UPalette, USound}
* 01772a83 Support for Lemony Snicket's A Series of Unfortunate Events data class {UProperty}
* c4c1978d Fixed support for Dungeon Defenders 2 (versions 687-688/111-117)
* 86538e5d Fixed support for Vanguard: Saga of Heroes
* eb82dba5 Fixed support for Rocket League (version 868/003)
* 6ed6ed74 Fixed support for Hawken (version 860/002)
* b4b79773 Fixed ResizeStringToken for UE1 builds
* 3653f8e1 Fixed ReturnToken and BeginFunctionToken for UE1 builds (with a package version of 61)
* 9a659549 Fixed deserialization of Heritages for UE1 builds (with a package version older than 68)

Notable changes that affect various data structures:

* Improved detection of UComponent objects and class types.
* ea3c1aa5 Support for UE4 .uasset packages (earlier builds only)
* e37b8a12 Support for class {UTexture}, f1b74af1 {UPrimitive, UTexture2D and its derivatives} (UE3)
* aa5ca861 Support for classes: {UFont, UMultiFont}
* ab290b6c Support for types {UPolys, FPoly}
* 02bea77b Support for types {FUntypedBulkData} (UE3) and {TLazyArray} (UE1, UE2)
* 94e02927 Support for structures: {FPointRegion, FCoords, FPlane, FScale, FSphere, FRotator, FVector, FGuid, FBox, FLinearColor, FMatrix, FQuat, FRange, FRangeVector, FVector2D, FVector4}
* 09c76240 Support for class {USoundGroup} (UE2.5)

**Support for the data types listed above have only been implemented for the standard structure that Epic Games uses**

## 1.3.5

* Fixed regression #48
* Fixed regression #46

## 1.3.4

* Fixed regression #45; (tools crash and launch by file crashes introduced with 1.3.3)

## 1.3.3

* Fixed regression #43
* Fixed #25; (UAC, all user-data and settings will now be saved to `%AppData%\EliotVU\UE Explorer`)

## 1.3.2

* Fixed issues with DPI scaling
* Fixed Hex-Viewer scrolling

## 1.3.1.0

This is a hotfix release which addresses a few minor issues with UE Explorer.

Notable changes:

* Updated to the latest UELib 1.3.1 (this includes fixes support for Batman, and XCOM series, among general decompilation fixes)
* Slightly tweaked the text editor theme
* Displaced the WebBrowser (homepage) with WebView2
* Updated and fixed some edge cases where "Disassemble tokens" would fail

## [1.3.0.0](https://eliotvu.com/forum/thread-508.html)

Please see [compare release-1.2.7.1...release-1.3.0.0](https://github.com/UE-Explorer/UE-Explorer/compare/release-1.2.7.1...release-1.3.0.0)

Notable changes:

* Updated .NET Framework from v4.0 to v4.8
* Updated UELib from 1.2.7.1 to [1.3.0.0](https://github.com/EliotVU/Unreal-Library/releases/tag/1.3.0.0)

Change log notes for UELib 1.3.0.0:

* The raw change log can be viewed here
* Support for Vengeance which includes BioShock 1 & 2, Swat4, and Tribes: Vengeance
* Support for Batman series (to the release branch, incomplete).
* Support for Thief: Deadly Shadows and Deus Ex: Invisible War
* Support for America's Army 2 (and Arcade)
* Support for Unreal II: eXpanded MultiPlayer
* Support for The Chronicles of Spellborn
* Improved general support for UE1 (Unreal 1), UE2 (Rainbow Six etc) & UE2.5, and later UE3 (esp UDK).
* Fixes to DefaultProperties

## [1.2.7.1](https://github.com/UE-Explorer/UE-Explorer/releases/tag/release-1.2.7.1)

Eliot.UELib.dll the engine behind UE Explorer is now open sourced at <https://github.com/EliotVU/Unreal-Library>

* Added support for Vanguard: Saga of Heroes.
* Added support for Tera: Rising.
* Added support for Mortal Kombat Komplete Edition.
* Added support for Transformers: Cybertron (and possible other releases).
* Updated support for SpecialForce2.

* Fixed an issue where certain fields were falsely decompiled and seen as replicated in the Replication block of a class.
* Fixed support for Shadow Complex.
* Fixed some localization errors.
* Fixed a case where UE Explorer may crash, when it opens a package for the first time.

## [1.2.6.0](https://eliotvu.com/forum/thread-92.html)

* Added an option to build the classes TreeView into a classes hierachy view:
![image](https://github.com/UE-Explorer/UE-Explorer/assets/808593/d83fba3b-0f8f-41db-895c-5ed7744a77c9)

Enabled BinaryMetaData in public build, this means a lot of data will be recorded while the package is being deserialized, this data can then be accessed through "View Buffer".

* Added the following new context menu features:

* (Node)View Outer
* (Node)View Parent
* (Text selection)Search in Document
* (Text selection)Search in Classes
* (Text selection)Search as Object

* Added defaultproperties support for the game "Dungeons & Dragons: Daggerdale"

* Added the function "ReturnValue" property to the "Parameters" node.
* Added "ScriptText", "CppText", "ProcessedText", "Within", "Super", and "NextField" as ObjectNodes in the classes TreeView where available.
* Added new interface icons and updated old icons to Visual Studio 2013

## [1.2.5.0](https://eliotvu.com/forum/thread-92.html)

* Added new View option "View Disassembled Tokens".

* Added state tracking, which remembers what object you decompiled the last time per-file basis. Upon loading a file you will be brought back to the last content state.

* Added :Replication, :Tokens, and :Tokens-Disassembled suffixes for the object search input form.

* Added configurable templates for :Tokens and :Tokens-Disassembled, you can now configure the output format by editing the files in "UE Explorer/Config/Templates/".

* Fixed an issue with "Swat 4" files.

## [1.2.4.0](https://eliotvu.com/forum/thread-76.html)

* Added support for Bioshock Infinite.
* Added support for Remember Me.
* Added support for Alpha Protocol.

* Fixed several bugs with UT3 and Mirrors Edge.
* Fixed an issue with Hex Viewer.

* Added a new feature to search objects by group. For example if you input Commandlet.Main then UE Explorer will search for the class Commandlet and output its Main function.

## [1.2.3.0](https://eliotvu.com/forum/thread-76.html)

Hex Viewer:

Open In -> Hex Workshop: It is now very easy and quick to open Hex Workshop with the opened file and auto scrolls to where the buffer is located.

![image](https://github.com/UE-Explorer/UE-Explorer/assets/808593/38eef2eb-2fe4-47a9-a460-1b96b61b0dcd)
Help FAQ.

View Tokens: A much more detailed output.

1. Shows the corresponding buffer of each statement.
2. Memory and Storage positioned, and basic structure explanation built-in so you won't forget!
3. Shortened token names.

Updated the Hex Viewer to highlight selected and hovered tokens. The pink is a JumpIfNot token's content, then the more purple/pink color is the IntConstToken's content.

Live editing and updating:

![image](https://github.com/UE-Explorer/UE-Explorer/assets/808593/35eaefca-3b8d-4d2e-aa14-dff49a731abf)

New Hex Viewer tools:

1. Copy Address, Copy Size.
2. Dump bytes will now replace the hyphen with a space delimiter.
3. Reload Package and Reload Buffer.

Other:
Auto-Updater: UE Explorer will now check for updates upon launch and give you the option to download and install with an ease.

New commandline arguments:

1. -newwindow (Opens a new window instance)
2. -silent (Only valid if -console is specified as well)

* Engine Licensee Version is now displayed if available(e.g. Borderlands 2).
* Recently Opened Files now displays its full path.
* Streams are now opened with shared writing, so you can edit using your favorite Hex Editor and then using the Hex Viewer in UE Explorer you can hit CTR + D to update and preview its changes immediately.
* Function calls with skipped parameters are now output correctly for UE3 packages.
* Some Memory versus Storage size fixes for certain tokens.
* Support for the new StateVariableToken and decompiling of Local variables in States.

## [1.2.2.1](https://eliotvu.com/forum/thread-38-post-330.html#pid330)

* Fixed the broken shortcut that was created by the setup program
* Added an copyright notice in About and decompiled classes
* Revised how arguments are handled, you are now required to append -console if you want to use export(which is now also -export)
* Fixed a crash when exporting classes through the commandline
* Fixed the installation of the NTL(Natives Tables List) extension

## [1.2.2.0](https://eliotvu.com/forum/thread-38.html)

Games:

* Supported Special Force 2
* Supported Borderlands 2
* Supported Stargate Worlds
* Supported Shadow Complex
* Supported XIII(Not perfect)
* Supported Hawken
* Supported Dishonored

Content:

* You can now export .WAV sounds from games such as UT2003 and UT2004, etc

Decompilation:

* Decompiled subobojects or archetypes will now have an reference printed below them
* Indentation can now be customized throughout the Options tab
* Decompiled functions whom are native will now have an reference printed above the declaration revealing its C++ declaration name
* CPPText and StructCPPText blocks are now decompiled if available
* Added a new feature in options, making it possible to define the structure of custom arrays, for example when an array fails to decompile you can go to options and define its structure and try again!
* Float values are no longer decompiled to the precision of 2 anymore. For example: 1.205 was decompiled to 1.20
* Function precedences are now decompiled for example prior update `(4 + 5) * 4` was decompiled as `4 + 5 * 4`;
* Default UStructProperties and UArrayProperties now fetch their associated property to properly determine the array type even if nested deeply
* \r and \n escape chars are now properly displayed for both string constants and default string values
* Array variables whom have a fixed size determined by an Enum constant are now decompiled as well
* Enum comparisons such as ENetMode and ENetRole are now replaced with their corresponding enum name
* Improved looping ifs and foreach iterations. Breaks, continues are now suppressed when necessary and printed in situtations they weren't previously
* NoExport specifier is now suppressed on UE3+ builds as it became redundant
* K2Call, K2Override, and K2Pure function specifiers are now decompiled
* Supported many more UnrealScript casting tokens such as StringToDelegate, ObjectToInterface, InterfaceToObject, and InterfaceToBool
* Better handling of buffer disposing and DeserializeOnDemand handling which means a much faster package loading process

Interface:

* Added an Context Menu for the Text Editor with the following choices: &ldquo;Copy&rdquo;, &ldquo;Search UnrealWiki for &#8230;&rdquo;
* Added &ldquo;Managed Properties&rdquo; to object nodes(This shows all the C# properties of a object, to be used for reading purposes only&quot;)
* New functionality for the Hex Viewer tool such as &ldquo;Copy Bytes&rdquo;, &ldquo;Copy View&rdquo;, &ldquo;Import Binary File&rdquo;, and &ldquo;Export Binary File&rdquo;
* Easier access to the context menu of already selected nodes
* Added &ldquo;Recent Opened Files&rdquo; under &ldquo;File&rdquo;
* You can now export classes/scripts using the commandline, for example: &quot;UE Explorer.exe&quot; &quot;FILEPATH&quot; export=classes(or scripts)
* Added Find in Document, Find in Classes, and Find Next. Including shortcuts
* Added new Context Menu shortcuts such as &quot;View Processed Script&quot;, &quot;View CPP Script&quot;, &quot;Table Buffer&quot;, and &quot;DefaultProperties Buffer&quot;
* Using the &quot;View Buffer&quot; on functions will now auto generate data structures for the functions code partition
* Supported the Dutch language
* Dontators of UE Explorer can now see their first name/nickname in the About dialog
* Contextmenu options for nodes such as &quot;View Tokens&quot; are now also accessible through the Exports/Content tabs.
* It&rsquo;s now possible to view the buffer of Import and Export tables
* Back and Forward now restores the Text Editor scrolled position
* Window size, state, and position is now restored for the Main Form and Hex Viewer Dialog
* Polished look of the overall application
* More and better lazy loading of tabs which means a much faster package loading process

Fixes:

* Corrected version compatibility for all package versions
* Gears of War 2(and many other games) classes are now properly deserialized(No longer rendered in red)
* Fixes for Find such as: restart from carret not from last find position
* Fixes for NO defaultproperties in Dungeon Defenders, Singularity, Crimecraft, Roboblitz, and UT3
* Fixed missing statements in the Replication block if a package was built with debug tokens
* Parameters with default values are now properly detected by using NothingTokens as skipped parameters
* Empty config specifiers no longer have parentheses
* (For UE3+ packages) decompilation of defaultproperties no longer stops after not finding an array's type

## [1.1](https://eliotvu.com/forum/thread-3.html)

Games:

* Supported APB: All Points Bulletin
* Supported Gears of War 2
* Supported Gears of War 3
* Supported Singularity
* Supported Roboblitz
* Supported NASA's Moonbase Alpha
* Supported Medal of Honor: Airborne(Reasonable)
* Revised Mirrors Effect

New:

* Packages with non-unreal signatures will now prompt a dialog with yes/no on whether you still want to load the package.
* Linking exceptions are now ignored.
* Exporting scripts now export the DefaultProperties as well if available.
* MetaData comments are now formatted as single-line and multi-line if detected.
* Reorganized the packages and options tab slightly.
* Struct references are no longer decompiled with outers e.g. "Core.Object.Vector" now becomes just "Vector".

* Added flag "Encrypted" for UT99.
* Added flags "Compressed" and "FullyCompressed" for UE3 packages.
* Added support for variable specifiers "EditHide" and "CrossLevelActive".
* Added support for param specifier "Init".
* Added support for class specifiers "ForceScriptOrder( true )" and "NativeOnly".
* Added support for code statement "FilterEditorOnly{...}".
* Added NativesTableList for UT3 and rebuilt the NativesTableList for UDK.

Fixes:

* Changed the "Package is cooked" warning to a "Compressed" warning.
* Better formatting for Switch/Case nests.
* Removed UDK-2011-04 NativesTableList, and replaced with UDK-2012-05 (04 was identical to 08).
* Fixed: Assert( ... ) tokens for packages above version 535(assumption based on Mirrors Edge).
* Fixed: PostOperators for Unreal Engine 3 games. PostOperators were detected as binary operators which caused the `++ )` issue.
* Fixed: State code for UT3 and other UE3 games.
* Fixed: "NoEditInlineNew" to "NotEditInlineNew".
* Fixed: Interface variables like "FIController" were decompiled as "interface".
* Fixed: Structs now output a semicolon at the end of the struct.

Prior release:

* Added a link button to this forum.
* Added a package reload button.
* Added a filtering box for Classes.
* Added configuration options so that you can customize the formatting of { and } curly brackets.
* Controls are now disabled when unusable

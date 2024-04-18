#

## 1.4.1

* Fixed #30 and #35; suppresses signature warning for Hawken and Killing Floor packages.
* Fixed regression #49; thanks to @Un-Drew
* Added a history navigation dropdown button #47
* QoL updates to the "Hex Viewer", such as:
** Copy Cell (value, size, offset; either in hex or decimal format)
** Edit Struct (allows one to edit names and objects by selecting an object using a dialog)
** Remove Struct (removes an applied struct)
* Updated to UELib 1.5.0 from 1.3.1

### UELib Changelog

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

## 1.3.0.0

Please see [compare release-1.2.7.1...release-1.3.0.0](https://github.com/UE-Explorer/UE-Explorer/compare/release-1.2.7.1...release-1.3.0.0)

Notable changes:

* Updated .NET Framework from v4.0 to v4.8
* Updated UELib from 1.2.7.1 to [1.3.0.0](https://github.com/EliotVU/Unreal-Library/releases/tag/1.3.0.0)

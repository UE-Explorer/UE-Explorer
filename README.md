<div align="center"><picture>
  <img src="https://user-images.githubusercontent.com/808593/170878374-0d8902e0-5688-4a71-b39f-b65ae64bf540.png" alt="UE Explorer" width="180"
</picture></div>

<div align="center">
    
**Package explorer tool for Unreal Engine (.upk, .u) with `UnrealScript` decompilation, browsing, and hex inspection**

[![Discord](https://img.shields.io/badge/Discord-Join%20Us-7289DA?logo=discord&logoColor=white)](https://discord.gg/8d4p3M2nMw)
[![Website](https://img.shields.io/badge/Website-UE%20Explorer-blue)](https://eliotvu.com/portfolio/ue-explorer)
[![Release](https://img.shields.io/github/release/UE-Explorer/UE-Explorer)]()
[![Downloads](https://img.shields.io/github/downloads/UE-Explorer/UE-Explorer/total?color=green)]()

![app demo](https://user-images.githubusercontent.com/808593/170879806-00b481c0-5f09-4c3b-bb12-56870b8d264f.png)

</div>


## Features

***UnrealScript Decompilation***:  
- High-accuracy UnrealScript decompilation
- Export individual classes or entire packages

***Package Exploration***:
- Class tree navigation for decompiled UnrealScript
- Content tree for inspecting object properties (pseudo `.T3D` format)
- Package dependency tree navigation (including referenced objects)

***Package Exporting***:
- Export any decompiled UnrealScript class to `.uc`
- Export any `Sound` and `SoundNodeWave` in its raw form i.e. a `.wav`

***Package Modding***:
- Structured Hex Viewer for inspecting and editing binary data
- UnrealScript bytecode token output for low-level analysis and hex modding workflows
  
***Engine Support***:
- Unreal Engine 1
- Unreal Engine 2, UE2.5
- Unreal Engine 3

*UE2X and UE4/5 have not been supported as of yet, except for limited experimental parsing of loose UE4 `.uasset` containers*

Note: Many games ship with engine modifications; compatibility may vary depending on custom forks or heavily altered packages.
A list of games that have been confirmed to work can be viewed [here](https://github.com/EliotVU/Unreal-Library).

## Install

You can download and install UE Explorer for **Windows** using one of the following sources:
- EliotVU: https://eliotvu.com/portfolio/ue-explorer/download/
- GitHub: https://github.com/UE-Explorer/UE-Explorer/releases
- winget: `winget install "ue explorer"`


## Support Development

UE Explorer and [UELib](https://github.com/EliotVU/Unreal-Library) are free and open source, and community support helps fund updates, fixes, and new features.

If UE Explorer helps your research, modding, or learning, please consider supporting its continued development 

[![EliotVU.com](https://img.shields.io/badge/EliotVU-Support-EA4AAA?style=for-the-badge&logo=githubsponsors&logoColor=white)](https://eliotvu.com/portfolio/ue-explorer/support/)

## Opening decompressed packages

In most cases you can open any `.upk`, `.u`, `.xxx` (compressed) etc. files anywhere, but, for **UE Explorer 1.6.1** and older, packages must be decompressed before hand.

In order to decompress a package you can use a third-party tool such as [Gildor's Unreal Package Decompressor](https://www.gildor.org/downloads)

* Drag and drop the compressed package on top of the `decompress.exe` executable.
* A new folder `unpacked` should have been created in the same directory as the executable.
* You now open the `.upk` file respectively from the `unpacked` directory using **UE Explorer**

*If the decompression failed then it it's likely that the compression codec could not be detected by the tool, or the tool has no support for that package format.*
*Using the commandline, you can append arguments to tell the tool what codec to use for decompression:*

* `-lzo|lzx|zlib` e.g. `decompress.exe -lzo "Core.upk"`
* `-game=<TAG>` (you can use `-taglist` to list all applicable tags) e.g. `decompress.exe -lzo -game=xcom2 "Core.upk"`
* `-ps3`, if necessary.

## Command-line interface

You can launch **UE Explorer** in console mode, by appending the `-console` argument to the `ueexplorer.exe` executable.

The program accepts the following arguments:

* `[file-path]` The package file path to open.
* `-console` Launches the program as a console window.
  * `-silent` The console will automatically close when it has finished its tasks.
  * `-export=classes|scripts` Exports all classes or scripts from the specified package file.
* `-newwindow` Launches the program in a new window, as opposed to a new tab in the currently running program.


## How to contribute

The project is built on the .NET Framework 4.8 WinForms library using C#.

If you want to contribute to the app you can do so by doing one of the following:
- Open an issue
- Or make a pull-request by creating a [fork](https://help.github.com/articles/fork-a-repo/) of this repository, create a new branch and commit your changes to that particular branch, so that I can easily merge your changes.

## How do I add support for a game?

This is the repository for the UI which is using UELib to do most of its Unreal related work.

See the [UELib](https://github.com/EliotVU/Unreal-Library) for more.


## Guides

UE Explorer has been widely used to dig into and mod Unreal Engine based games.
Various communities have written guides, such as:

* [Borderlands Modding](https://github.com/BLCM/BLCMods/wiki)
* [Dishonored Ultimate Difficulty Mod](https://www.ttlg.com/forums/showthread.php?t=141188&page=2&p=2208847&viewfull=1#post2208847)
* [Aliens Colonial Marines: Editing game variables](https://www.moddb.com/games/aliens-colonial-marines/tutorials/aliens-colonial-marines-editing-game-variables)
* [Modding Guide - Gal*Gun Double Peace](https://steamcommunity.com/sharedfiles/filedetails/?id=1241233230)
* [Batman Arkham City - Hacking Unrealscript](https://www.youtube.com/watch?v=aEvoWFlvIQs)

*Do you have or know of a guide that's missing here? Feel free to submit a pull-request, inform us in the discussion board*

Furthermore, many communities have been spun up that make extensive use of UE Explorer:

* [Arkham Workshop](https://discord.gg/N7buKT82), [Graphics Processing Community](https://discord.gg/graphicsprocessingunity-963091199739166760) - A Batman series modding community
* [Bioshock Modding Hub](https://discord.gg/djmWv4HwZZ)
* [Borderlands Modding](https://discord.gg/E47p2Q8gsa)
* [Infinity Blade: Modding](https://discord.gg/uQRgZAFw34)
* [ReEnergized Community](https://discord.gg/7CFaqbBCXx) - A Transformers modding community
* [Unreal Engine Modding](https://discord.gg/eRJrfyG9Ap)


## Credits

- [Antonio Cordero Balcazar](https://github.com/acorderob) for [UTPT](https://www.acordero.org/projects/unreal-tournament-package-tool)

This project is an independent, community-developed tool and is not affiliated in any way with, endorsed by, or sponsored by Epic Games, Inc. Unreal, Unreal Engine, UDK, and the Unreal U logo are property of Epic Games, Inc.

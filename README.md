<p align="center">
  <img src="https://user-images.githubusercontent.com/808593/170878374-0d8902e0-5688-4a71-b39f-b65ae64bf540.png" width="180"
</p>

# UE Explorer 

[UE Explorer](https://eliotvu.com/portfolio/view/21/ue-explorer) is an easy-to-use browser and decompiler for Unreal Engine packages (.upk, .u).
  
![app](https://user-images.githubusercontent.com/808593/170879806-00b481c0-5f09-4c3b-bb12-56870b8d264f.png)

## Install

You can download and install UE Explorer for **Windows** using one of the following sources:
- winget: `winget install "ue explorer"`
- EliotVU: https://eliotvu.com/portfolio/download/21/ue-explorer
- GitHub: https://github.com/UE-Explorer/UE-Explorer/releases

## Features
  
- UnrealScript decompilation with high accurracy.
- Export any UnrealScript classes, or entire package of classes.
- Export any sound of an Unreal package.

Exploring:
- Explore a tree of classes to navigate all decompiled UnrealScript classes.
- Explore a tree of content to navigate the properties of any non-UnrealScript object.
- View all the dependencies of an Unreal package, including the object that it is dependant on.

Modding:
- Hex Viewer with rich defined-structures to help with debugging and/or hex-modding.
- A specialized UnrealScript tokens output to assist with hex-modding.
  
It has support for:
- Unreal Engine 1
- Unreal Engine 2, UE2.5
- Unreal Engine 3

*UE2X and UE4/5 have not been supported as of yet*

However many games may have modified the engine to some extent.

A list of games that have been confirmed to work can be viewed [here](https://github.com/EliotVU/Unreal-Library).

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

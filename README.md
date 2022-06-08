<p align="center">
  <img src="https://user-images.githubusercontent.com/808593/170878374-0d8902e0-5688-4a71-b39f-b65ae64bf540.png" width="180"
</p>

# UE Explorer 

[UE Explorer](https://eliotvu.com/portfolio/view/21/ue-explorer) is an easy-to-use browser and decompiler for Unreal Engine packages (.upk, .u).
  
![app](https://user-images.githubusercontent.com/808593/170879806-00b481c0-5f09-4c3b-bb12-56870b8d264f.png)

It has support for:
  * Unreal Engine 1
  * Unreal Engine 2, UE2.5
  * Unreal Engine 3
  * UE2X and UE4/5 have not been supported as of yet.

However many games may have modified the engine to some extent.

A list of games that have been confirmed to work can be viewed [here](https://github.com/EliotVU/Unreal-Library).
  
# Features
  
  * UnrealScript decompilation with high accurracy
  * Package classes tree
  * Package dependencies tree
  * Package content tree
  * Hex Viewer with rich defined-structures to help with debugging and/or hex-modding
  * Specialized tokens output to help with hex-modding
  * Code export
  * ... TODO
  
# How to contribute

The project is built on the .NET Framework 4.8 WinForms library using C#.

If you want to contribute to the app you can do so by doing one of the following:
* Open an issue
* Or make a pull-request by creating a [fork](https://help.github.com/articles/fork-a-repo/) of this repository, create a new branch and commit your changes to that particular branch, so that I can easily merge your changes.

# How do I add support for a game?

This is the repository for the UI which is using UELib to do most of its Unreal related work.

See the [UELib](https://github.com/EliotVU/Unreal-Library) for more.
  
# Credits
  
  * [Antonio Cordero Balcazar](https://github.com/acorderob) for [UTPT](https://www.acordero.org/projects/unreal-tournament-package-tool)

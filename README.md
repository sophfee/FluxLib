# FluxLib
A mod for [Atlyss](https://store.steampowered.com/app/2768430/ATLYSS/) that aims to provide easy to digest methods to create custom content for mod creators.

## What can I do?
With the current version of FluxLib, you can...

- [x] Create custom trade items.
- [x] Add custom items to vendors.
- [ ] Create custom consumables.
- [ ] Create custom conditions.
- [ ] Create custom quests, with custom quest requirements.
- [ ] Append existing loot tables, to make custom items droppable from existing creeps.

More goals will be added to this list, but this is what I am currently aiming to achieve! 

## How can I use this mod?
It's fairly simple, and everything is built around the JSON file format. Upon launching the mod once, you will have a new folder in your Atlyss directory called `Flux`. Inside this folder, you will find several directories, these directories are used to load custom objects into the game, subfolders will be read when parsing.  
The `Assets` subfolder is used to load custom textures into the game, there is no current support for mesh loading, but I will work on it in the near future! The system will most likely depend on [AssetBundles](https://docs.unity3d.com/Manual/AssetBundlesIntro.html) to load custom meshes into the game.  
The `Examples` folder in this repository contains examples of how to create custom items, this will be more thoroughly documented in the future.
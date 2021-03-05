# Rogue Ascension

## About this repo
At any given time this repo should hold the latest version of the project. If you'd like to add to the project, you can download the Github for Unity package over here -> https://unity.github.com/ and then connect to this project!

Alternatively, you can download the source, make the changes/prefabs you'd like to add, and then send it over as a unity package. 

## Some notes on making new rooms

You can find some room prefabs already made in Assets/Prefabs/Rooms. When making new rooms, it's best to duplicate one of them, then add assets from the Assets/Prefabs/Rooms/Room Pieces folder.

To add them to the level generation, you can add them to the Dungeon Generator component on the GameMaster object as detailed below.

## The Dungeon Generator

There's lots of parameters to play with on the dungeon generator, which is found on the GameMaster object.

#### Dungeon Parameters

**Dungeon Width/Height** - How many rooms wide/tall the level will be.

**Room Width/Height** - How wide/tall each room is in Unity units. All rooms on a given floor need to be the same size!

**Shops to Spawn** - How many shops total will spawn in the level.

**Bonus Rooms to Spawn** - How many bonus rooms should spawn at maximum. Less than this number may spawn if there isn't room for them. 

**Seed** - The seed used when generating. 

#### Prefabs

**Four Way Rooms** - Rooms with openings on all four sides.

**Horizontal Rooms** - Rooms with left and right exits.

**Vertical Rooms** - Rooms with top and bottom exits.

**Shops** - The pool of shops to spawn when generating.

**Bonus Rooms** - Rooms that will be spawned at random points outside of the main solution path.

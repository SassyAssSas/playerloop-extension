<!-- omit from toc -->
# PlayerLoop Extension
Provides a convenient way of editing Unity's PlayerLoop.

<!-- omit from toc -->
## Table of contents
- [Installation](#installation)
- [PlayerLoopBuilder](#playerloopbuilder)
- [PlayerLoopSystem extension methods](#playerloopsystem-extension-methods)
- [Editor mode nuanses](#editor-mode-nuanses)

## Installation
To start using the package, go to [releases page](https://github.com/SassyAssSas/playerloop-extension/releases) and get the release of your choice.

## PlayerLoopBuilder
To start working with the library use the `Violoncello.PlayerLoopExtensions` namespace.

To edit the PlayerLoop use `PlayerLoopBuilder` class. There are 3 ways to get an instance of `PlayerLoopBuilder`. You can see them all on the example below:
```csharp
// Will return a new instance PlayerLoopBuilder with empty player loop:
PlayerLoopBuilder.FromNew();

// Will return a new instance PLayerLoopBuilder that already contains default PlayerLoop systems:
PlayerLoopBuilder.FromDefault();

// Will return a new instance PLayerLoopBuilder that contains all the current PlayerLoop systems:
PlayerLoopBuilder.FromCurrent();
``` 

`PlayerLoopBuilder` object is used to add and remove systems in the PlayerLoop using methods shown on the example below:
```csharp
// All these methods return the instance of PlayerLoopBuilder
// So you can use many of them in a row
PlayerLoopBuilder.FromCurrent()
                 .AddToRoot(mySystem)
                 .AddToSubSystem<Update>(mySystem)
                 .AddToRoot<MySystem>(Callback) 
                 .AddToSubSystem<Update, MySystem>(Callback)
                 .RemoveFromRoot<MySystem>()
                 .RemoveFromSubSystem<Update, MySystem>()
                 .RemoveFromRoot(typeof(MySystem))
                 .RemoveFromSubSystem(typeof(Update), typeof(MySystem));
```
When you finish working with the builder you can either call the `PlayerLoopBuilder.Build` method to get a result `PlayerLoopSystem`:
```csharp
var playerLoop = PlayerLoopBuilder.FromCurrent()
                                  .AddToSubSystem<Update, MyUpdate>(MyUpdateCallback)
                                  .AddToSubSystem<FixedUpdate, MyFixedUpdate>()
                                  .Build();

// Will set the result PlayerLoopSystem as unity's main PlayerLoop 
PlayerLoop.SetPlayerLoop();
```
Or call `PlayerLoopBuilder.SetPlayerLoop()` which will set the result `PlayerLoopSystem` as unity's default PlayerLoop without you having to retrieve the result `PlayerLoopSystem` and doing it yourself: 
```csharp
var playerLoop = PlayerLoopBuilder.FromCurrent()
                                  .AddToSubSystem<Update, MyUpdate>(MyUpdateCallback)
                                  .AddToSubSystem<FixedUpdate, MyFixedUpdate>()
                                  .SetPlayerLoop();
```

## PlayerLoopSystem extension methods
If you need to edit a `PlayerLoopSystem`, you might make a use of it's new extension methods:
```csharp
// Searches for a subSystem with the passed type and returns it.
// Throws an exception if doesn't find the subSystem
playerLoopSystem.FindSubSystem<MySystem>();
playerLoopSystem.FindSubSystem(typeof(MySystem));

// Searches for a subSystem with the passed type and puts it in the out variable.
// Returns true if the subSystem was found, otherwise returns false
playerLoopSystem.TryFindSubSystem<MySystem>(out PlayerLoopSystem system);
playerLoopSystem.TryFindSubSystem(out PlayerLoopSystem system, typeof(MySystem));

// Adds a subSystem
playerLoopSystem.AddSubSystem(mySystem);

// Creates a new subSystem using passed type and callback and adds it
playerLoopSystem.AddSubSystem<MySystem>(Callback);

// Removes all subSystems with passed type
playerLoopSystem.RemoveSubSystem<MySystem>();
playerLoopSystem.RemoveSubSystem(typeof(MySystem));

// Searches for a subSystem with the passed type and replaces it
playerLoopSystem.ReplaceSubSystem<OtherSystem>(mySystem);
playerLoopSystem.ReplaceSubSystem(Callback, typeof(OtherSystem));
```

## Editor mode nuanses
By default all the added systems keep working even if you exit play mode. `PlayerLoopBuilder` saves all the systems you add and removes them on play mode exit. If you want to disable this behaviour, while adding a new system pass `false` as a second argument:
```csharp
// This subsystem won't be automatically removed
PlayerLoopBuilder.FromCurrent()
                 .AddToSubSystem<Update>(mySystem, false)
                 .SetPlayerLoop();
```
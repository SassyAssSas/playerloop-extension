<!-- omit from toc -->
# PlayerLoop Extension
Provides a convinient way of editing Unity's PlayerLoop.

<!-- omit from toc -->
## Table of contents
- [PlayerLoopBuilder](#playerloopbuilder)
- [PlayerLoopSystem extension methods](#playerloopsystem-extension-methods)
- [Tests](#tests)

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
                 // Add methods requre an instance of PlayerLoopSystem
                 .AddToRoot(mySystem)
                 .AddToSubSystem<Update>(mySystem)

                 // You might pass a type and a callback method if you don't want
                 // To create an instance of PlayerLoopSystem yourself
                 .AddToRoot<MySystem>(Callback) 
                 .AddToSubSystem<Update, MySystem>(Callback)

                 // Remove systems using generic types 
                 .RemoveFromRoot<MySystem>()
                 .RemoveFromSubSystem<Update, MySystem>()

                 // Remove systems using Type variable
                 .RemoveFromRoot(typeof(MySystem))
                 .RemoveFromSubSystem(typeof(Update), typeof(MySystem));
```

## PlayerLoopSystem extension methods
If you need to edit a `PlayerLoopSystem`, you might make a use of their new extension methods:
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

## Tests
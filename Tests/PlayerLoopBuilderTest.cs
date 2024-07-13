using System.Linq;
using NUnit.Framework;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Violoncello.PlayerLoopExtensions.Tests {
   internal class PlayerLoopBuilderTest {
      [Test]
      public void CreateFromNew() {
         var a = new PlayerLoopSystem();
         var b = PlayerLoopBuilder.FromNew()
                                  .Build();

         Assert.IsTrue(a.PlayerLoopEquals(b));
      }

      [Test]
      public void CreateFromDefault() {
         var a = PlayerLoop.GetDefaultPlayerLoop();
         var b = PlayerLoopBuilder.FromDefault()
                                  .Build();

         Assert.IsTrue(a.PlayerLoopEquals(b));
      }

      [Test]
      public void CreateFromCurrent() {
         var a = PlayerLoop.GetCurrentPlayerLoop();
         var b = PlayerLoopBuilder.FromCurrent()
                                  .Build();

         Assert.IsTrue(a.PlayerLoopEquals(b));
      }

      [Test]
      public void AddToRootPredefinedPlayerLoopSystem() {
         var mySystem = new PlayerLoopSystem() {
            type = typeof(MySystem)
         };

         var playerLoopSystem = PlayerLoopBuilder.FromDefault()
                                                 .AddToRoot(mySystem)
                                                 .Build();

         Assert.IsTrue(playerLoopSystem.subSystemList.Any(system => system.type == typeof(MySystem)));
      }

      [Test]
      public void AddToRootByTypeAndCallback() {
         var playerLoopSystem = PlayerLoopBuilder.FromDefault()
                                                 .AddToRoot<MySystem>(() => { })
                                                 .Build();

         Assert.IsTrue(playerLoopSystem.subSystemList.Any(system => system.type == typeof(MySystem)));
      }

      [Test]
      public void AddToSubSystemPredefinedPlayerLoopSystem() {
         var mySystem = new PlayerLoopSystem() {
            type = typeof(MySystem)
         };

         var playerLoopSystem = PlayerLoopBuilder.FromDefault()
                                                 .AddToSubSystem<Update>(mySystem)
                                                 .Build();

         var updateSystem = playerLoopSystem.subSystemList.First(system => system.type == typeof(Update));

         Assert.IsTrue(updateSystem.subSystemList.Any(system => system.type == typeof(MySystem)));
      }

      [Test]
      public void AddToSubSystemByTypeAndCallback() {
         var playerLoopSystem = PlayerLoopBuilder.FromDefault()
                                                 .AddToSubSystem<Update, MySystem>(() => { })
                                                 .Build();

         var updateSystem = playerLoopSystem.subSystemList.First(system => system.type == typeof(Update));

         Assert.IsTrue(updateSystem.subSystemList.Any(system => system.type == typeof(MySystem)));
      }

      [Test]
      public void RemoveFromRoot() {
         var playerLoopSystem = PlayerLoopBuilder.FromDefault()
                                                 .RemoveFromRoot<Update>()
                                                 .Build();

         Assert.IsFalse(playerLoopSystem.subSystemList.Any(system => system.type == typeof(MySystem)));
      }

      [Test]
      public void RemoveFromSubsystem() {
         var playerLoopSystem = PlayerLoopBuilder.FromDefault()
                                                 .RemoveFromSubSystem<Update, Update.DirectorUpdate>()
                                                 .Build();

         var updateSystem = playerLoopSystem.subSystemList.First(system => system.type == typeof(Update));

         Assert.IsFalse(updateSystem.subSystemList.Any(system => system.type == typeof(Update.DirectorUpdate)));
      }

      private struct MySystem {

      }
   }
}

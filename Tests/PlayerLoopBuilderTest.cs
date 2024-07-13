using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Violoncello.PlayerLoopExtensions.Tests {
   public class PlayerLoopBuilderTest {
      [Test]
      public void CreateFromNew() {
         var a = new PlayerLoopSystem();
         var b = PlayerLoopBuilder.FromNew()
                                  .Build();

         Assert.IsTrue(PlayerLoopEquals(a, b));
      }

      [Test]
      public void CreateFromDefault() {
         var a = PlayerLoop.GetDefaultPlayerLoop();
         var b = PlayerLoopBuilder.FromDefault()
                                  .Build();

         Assert.IsTrue(PlayerLoopEquals(a, b));
      }

      [Test]
      public void CreateFromCurrent() {
         var a = PlayerLoop.GetCurrentPlayerLoop();
         var b = PlayerLoopBuilder.FromCurrent()
                                  .Build();

         Assert.IsTrue(PlayerLoopEquals(a, b));
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


      private bool PlayerLoopEquals(PlayerLoopSystem a, PlayerLoopSystem b) {
         if (a.type != b.type) {
            return false;
         }

         if (a.subSystemList?.Length != b.subSystemList?.Length) {
            return false;
         }

         for (int i = 0; i < a.subSystemList?.Length; i++) {
            if (!PlayerLoopEquals(a.subSystemList[i], b.subSystemList[i])) {
               return false;
            }
         }

         return true;
      }

      private struct MySystem {

      }
   }
}

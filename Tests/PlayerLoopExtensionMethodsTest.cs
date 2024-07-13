using System;
using System.Linq;
using NUnit.Framework;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Violoncello.PlayerLoopExtensions.Tests {
   internal class PlayerLoopExtensionMethodsTest {
      [Test]
      public void FindSubSystemGenericSuccess() {
         var updateSystem = GetDefaultUpdateSystem();

         var directorUpdate = updateSystem.FindSubSystem<Update.DirectorUpdate>();

         Assert.IsTrue(directorUpdate.PlayerLoopEquals(GetDirectorUpdateSystem()));
      }

      [Test]
      public void FindSubSystemGenericFail() {
         var updateSystem = GetDefaultUpdateSystem();

         Assert.Throws<PlayerLoopSubSystemNotFoundException>(() => updateSystem.FindSubSystem<MySystem>());
      }

      [Test]
      public void FindSubSystemTypeArgumentSuccess() {
         var updateSystem = GetDefaultUpdateSystem();

         var directorUpdate = updateSystem.FindSubSystem(typeof(Update.DirectorUpdate));

         Assert.IsTrue(directorUpdate.PlayerLoopEquals(GetDirectorUpdateSystem()));
      }

      [Test]
      public void FindSubSystemTypeArgumentFail() {
         var updateSystem = GetDefaultUpdateSystem();

         Assert.Throws<PlayerLoopSubSystemNotFoundException>(() => updateSystem.FindSubSystem(typeof(MySystem)));
      }

      [Test]
      public void TryFindSubSystemGenericSuccess() {
         var updateSystem = GetDefaultUpdateSystem();

         Assert.IsTrue(updateSystem.TryFindSubSystem<Update.DirectorUpdate>(out PlayerLoopSystem directorUpdate));

         Assert.IsTrue(directorUpdate.PlayerLoopEquals(GetDirectorUpdateSystem()));
      }

      [Test]
      public void TryFindSubSystemGenericFail() {
         var updateSystem = GetDefaultUpdateSystem();

         Assert.IsFalse(updateSystem.TryFindSubSystem<MySystem>(out PlayerLoopSystem directorUpdate));

         Assert.IsTrue(directorUpdate.PlayerLoopEquals(default));
      }

      [Test]
      public void TryFindSubSystemTypeArgumentSuccess() {
         var updateSystem = GetDefaultUpdateSystem();

         Assert.IsTrue(updateSystem.TryFindSubSystem(out PlayerLoopSystem directorUpdate, typeof(Update.DirectorUpdate)));

         Assert.IsTrue(directorUpdate.PlayerLoopEquals(GetDirectorUpdateSystem()));
      }

      [Test]
      public void TryFindSubSystemTypeArgumentFail() {
         var updateSystem = GetDefaultUpdateSystem();

         Assert.IsFalse(updateSystem.TryFindSubSystem(out PlayerLoopSystem directorUpdate, typeof(MySystem)));

         Assert.IsTrue(directorUpdate.PlayerLoopEquals(default));
      }

      [Test]
      public void AddSubSystemWithPredefinedVariable() {
         var updateSystem = GetDefaultUpdateSystem();

         updateSystem.AddSubSystem(GetMySystem());

         Assert.DoesNotThrow(() => updateSystem.subSystemList.FirstOrDefault(system => system.type == typeof(MySystem)));
      }

      [Test]
      public void AddSubSystemWithTypeAndCallback() {
         var updateSystem = GetDefaultUpdateSystem();

         updateSystem.AddSubSystem<MySystem>(() => { });

         Assert.DoesNotThrow(() => updateSystem.subSystemList.FirstOrDefault(system => system.type == typeof(MySystem)));
      }

      [Test]
      public void RemoveSubSystemGeneric() {
         var updateSystem = GetDefaultUpdateSystem();

         updateSystem.RemoveSubSystem<Update.DirectorUpdate>();

         Assert.Throws<InvalidOperationException>(() => updateSystem.subSystemList.First(system => system.type == typeof(Update.DirectorUpdate)));
      }

      [Test]
      public void RemoveSubSystemTypeArgument() {
         var updateSystem = GetDefaultUpdateSystem();

         updateSystem.RemoveSubSystem(typeof(Update.DirectorUpdate));

         Assert.Throws<InvalidOperationException>(() => updateSystem.subSystemList.First(system => system.type == typeof(Update.DirectorUpdate)));
      }

      [Test]
      public void ReplaceSubSystemGeneric() {
         var updateSystem = GetDefaultUpdateSystem();

         updateSystem.ReplaceSubSystem<Update.DirectorUpdate>(GetMySystem());

         Assert.Throws<InvalidOperationException>(() => updateSystem.subSystemList.First(system => system.type == typeof(Update.DirectorUpdate)));

         Assert.DoesNotThrow(() => updateSystem.subSystemList.First(system => system.type == typeof(MySystem)));
      }

      [Test]
      public void ReplaceSubSystemTypeArgument() {
         var updateSystem = GetDefaultUpdateSystem();

         updateSystem.ReplaceSubSystem(GetMySystem(), typeof(Update.DirectorUpdate));

         Assert.Throws<InvalidOperationException>(() => updateSystem.subSystemList.First(system => system.type == typeof(Update.DirectorUpdate)));

         Assert.DoesNotThrow(() => updateSystem.subSystemList.First(system => system.type == typeof(MySystem)));
      }

      private PlayerLoopSystem GetDefaultUpdateSystem() {
         return PlayerLoop.GetDefaultPlayerLoop().subSystemList.First(system => system.type == typeof(Update));
      }

      private PlayerLoopSystem GetDirectorUpdateSystem() {
         return GetDefaultUpdateSystem().subSystemList.First(system => system.type == typeof(Update.DirectorUpdate));
      }

      private PlayerLoopSystem GetMySystem() {
         return new PlayerLoopSystem() {
            type = typeof(MySystem),
         };
      }

      private struct MySystem {
         
      }
   }
}

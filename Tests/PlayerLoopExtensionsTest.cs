using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.TestTools;

public class PlayerLoopExtensionsTest {
   [Test]
   public void PlayerLoopBuilderAddToRootPredefinedPlayerLoopSystem() {
      
   }

   [Test]
   public void PlayerLoopBuilderAddToRootByTypeAndCallback() {
      
   }

   // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
   // `yield return null;` to skip a frame.
   [UnityTest]
   public IEnumerator RoutineTestWithEnumeratorPasses() {
      // Use the Assert class to test conditions.
      // Use yield to skip a frame.
      yield return null;
   }
}

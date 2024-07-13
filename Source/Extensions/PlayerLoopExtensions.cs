using System;
using System.Linq;
using UnityEngine.LowLevel;

namespace Violoncello.PlayerLoopExtensions {
   public static class PlayerLoopExtensions {
      /// <summary>
      /// Searches for a subSystem with the specified type
      /// </summary>
      /// <typeparam name="T">SubSystem type</typeparam>
      /// <returns>Found PlayerLoopSystem reference</returns>
      /// <exception cref="PlayerLoopSubSystemNotFoundException">Thrown if the root system does't have a subSystem with given type</exception>
      public static ref PlayerLoopSystem FindSubSystem<T>(ref this PlayerLoopSystem root) {
         if (root.subSystemList == null) {
            throw new PlayerLoopSubSystemNotFoundException($"Couldn't find {typeof(T)} PlayerLoopSystem. The root system doesn't have any subSystems");
         }

         for (int i = 0; i < root.subSystemList.Length; i++) {
            if (root.subSystemList[i].type == typeof(T)) {
               return ref root.subSystemList[i];
            }
         }

         throw new PlayerLoopSubSystemNotFoundException($"Couldn't find {typeof(T)} PlayerLoopSystem.");
      }

      /// <summary>
      /// Searches for a subSystem with the specified type
      /// </summary>
      /// <returns>Found PlayerLoopSystem reference</returns>
      /// <param name="type">The type of a subSystem</param>
      /// <exception cref="PlayerLoopSubSystemNotFoundException">Thrown if the root system does't have a subSystem with given type</exception>
      public static ref PlayerLoopSystem FindSubSystem(ref this PlayerLoopSystem root, Type type) {
         if (root.subSystemList == null) {
            throw new PlayerLoopSubSystemNotFoundException($"Couldn't find {type} PlayerLoopSystem. The root system doesn't have any subSystems");
         }

         for (int i = 0; i < root.subSystemList.Length; i++) {
            if (root.subSystemList[i].type == type) {
               return ref root.subSystemList[i];
            }
         }

         throw new PlayerLoopSubSystemNotFoundException($"Couldn't find {type} PlayerLoopSystem.");
      }

      /// <summary>
      /// Tries to find a subSystem with the specified type and put it in the out variable
      /// </summary>
      /// <returns>true if the subSystem was found, otherwise false</returns>
      /// <param name="subsystem">Found subSystem</param>
      public static bool TryFindSubSystem<T>(ref this PlayerLoopSystem root, out PlayerLoopSystem subsystem) {
         if (root.type == typeof(T)) {
            subsystem = root;
            return true;
         }

         if (root.subSystemList != null) {
            for (int i = 0; i < root.subSystemList.Length; i++) {
               if (TryFindSubSystem<T>(ref root.subSystemList[i], out subsystem)) {
                  return true;
               }
            }
         }

         subsystem = default;

         return false;
      }

      /// <summary>
      /// Tries to find a subSystem with the specified type and put it in the out variable
      /// </summary>
      /// <returns>true if the subSystem was found, otherwise false</returns>
      /// <param name="subsystem">Found subSystem</param>
      /// <param name="type">The type of a subSystem</param>
      public static bool TryFindSubSystem(ref this PlayerLoopSystem root, out PlayerLoopSystem subsystem, Type type) {
         if (root.type == type) {
            subsystem = root;
            return true;
         }

         if (root.subSystemList != null) {
            for (int i = 0; i < root.subSystemList.Length; i++) {
               if (TryFindSubSystem(ref root.subSystemList[i], out subsystem, type)) {
                  return true;
               }
            }
         }

         subsystem = default;

         return false;
      }

      /// <summary>
      /// Adds new subSystem
      /// </summary>
      /// <param name="subsystem">SubSystem to add</param>
      public static void AddSubSystem(ref this PlayerLoopSystem root, PlayerLoopSystem subsystem) {
         Array.Resize(ref root.subSystemList, root.subSystemList.Length + 1);

         root.subSystemList[^1] = subsystem;
      }

      /// <summary>
      /// Creates new subSystem with the passed type and updateDelegate callback
      /// </summary>
      /// <param name="updateDelegate">UpdateDelegate callback</param>
      public static void AddSubSystem<T>(ref this PlayerLoopSystem root, PlayerLoopSystem.UpdateFunction updateDelegate) {
         var subsystem = new PlayerLoopSystem() {
            type = typeof(T),
            updateDelegate = updateDelegate
         };

         Array.Resize(ref root.subSystemList, root.subSystemList.Length + 1);

         root.subSystemList[^1] = subsystem;
      }

      /// <summary>
      /// Removes subSystem with the given type
      /// </summary>
      /// <param name="type">SubSystem type</param>
      public static void RemoveSubSystem(ref this PlayerLoopSystem root, Type type) {
         var list = root.subSystemList.ToList();

         list.RemoveAll((subsystem) => subsystem.type == type);

         root.subSystemList = list.ToArray();
      }

      /// <summary>
      /// Removes subSystem with the given type
      /// </summary>
      /// <param name="type">SubSystem type</param>
      public static void RemoveSubSystem<T>(ref this PlayerLoopSystem root) {
         var list = root.subSystemList.ToList();

         list.RemoveAll((subsystem) => subsystem.type == typeof(T));

         root.subSystemList = list.ToArray();
      }

      /// <summary>
      /// Replaces subSystem with the given type by another subSystem
      /// </summary>
      /// <param name="newSystem">New SubSystem</param>
      public static void ReplaceSubSystem<T>(ref this PlayerLoopSystem root, PlayerLoopSystem newSystem) {
         for (int i = 0; i < root.subSystemList.Length; i++) {
            if (root.subSystemList[i].type == typeof(T)) {
               root.subSystemList[i] = newSystem;
               return;
            }
         }
      }

      /// <summary>
      /// Replaces subSystem with the given type by another subSystem
      /// </summary>
      /// <param name="type">SubSystem type</param>
      /// <param name="newSystem">New SubSystem</param>
      public static void ReplaceSubSystem(ref this PlayerLoopSystem root, PlayerLoopSystem newSystem, Type type) {
         for (int i = 0; i < root.subSystemList.Length; i++) {
            if (root.subSystemList[i].type == type) {
               root.subSystemList[i] = newSystem;
               return;
            }
         }
      }

      public static bool PlayerLoopEquals(this PlayerLoopSystem a, PlayerLoopSystem b) {
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
   }
}

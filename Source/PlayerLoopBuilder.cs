using System;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Violoncello.PlayerLoopExtensions {
   public class PlayerLoopBuilder {
      private PlayerLoopSystem _playerLoopSystem;

      private PlayerLoopBuilder(PlayerLoopSystem playerLoopSystem) {
         _playerLoopSystem = playerLoopSystem;
      }

      public static PlayerLoopBuilder FromDefault() {
         return new PlayerLoopBuilder(PlayerLoop.GetDefaultPlayerLoop());
      }

      public static PlayerLoopBuilder FromCurrent() {
         return new PlayerLoopBuilder(PlayerLoop.GetCurrentPlayerLoop());
      }

      public static PlayerLoopBuilder FromNew() {
         return new PlayerLoopBuilder(new PlayerLoopSystem());
      }

      public PlayerLoopSystem Build() {
         return _playerLoopSystem;
      }

      public void SetPlayerLoop() {
         PlayerLoop.SetPlayerLoop(_playerLoopSystem);
      }

      public PlayerLoopBuilder AddToRoot(PlayerLoopSystem system, bool removeOnApplicationQuit = true) {
         _playerLoopSystem.AddSubSystem(system);

         if (removeOnApplicationQuit) {
            Application.quitting += () => FromCurrent().RemoveFromRoot(system.type).SetPlayerLoop();                                  
         }

         return this;
      }

      public PlayerLoopBuilder AddToRoot<TSystem>(PlayerLoopSystem.UpdateFunction updateDelegate, bool removeOnApplicationQuit = true) {
         _playerLoopSystem.AddSubSystem<TSystem>(updateDelegate);

         if (removeOnApplicationQuit) {
            Application.quitting += () => FromCurrent().RemoveFromRoot<TSystem>().SetPlayerLoop();
         }

         return this;
      }

      public PlayerLoopBuilder AddToSubSystem<TParent>(PlayerLoopSystem system, bool removeOnApplicationQuit = true) {
         _playerLoopSystem.FindSubSystem<TParent>()
                          .AddSubSystem(system);

         if (removeOnApplicationQuit) {
            Application.quitting += () => FromCurrent().RemoveFromSubsystem(typeof(TParent), system.type).SetPlayerLoop();
         }

         return this;
      }
      
      public PlayerLoopBuilder AddToSubsystem<TParent, TSystem>(PlayerLoopSystem.UpdateFunction updateDelegate, bool removeOnApplicationQuit = true) {
         _playerLoopSystem.FindSubSystem<TParent>()
                          .AddSubSystem<TSystem>(updateDelegate);

         if (removeOnApplicationQuit) {
            Application.quitting += () => FromCurrent().RemoveFromSubsystem(typeof(TParent), typeof(TSystem)).SetPlayerLoop();
         }

         return this;
      }

      public PlayerLoopBuilder RemoveFromRoot<TSystem>() {
         _playerLoopSystem.RemoveSubSystem<TSystem>();

         return this;
      }

      public PlayerLoopBuilder RemoveFromRoot(Type systemType) {
         _playerLoopSystem.RemoveSubSystem(systemType);

         return this;
      }

      public PlayerLoopBuilder RemoveFromSubsystem<TParent, TSystem>() {
         _playerLoopSystem.TryFindSubSystem<TParent>(out PlayerLoopSystem parent);

         parent.RemoveSubSystem<TSystem>();

         _playerLoopSystem.ReplaceSubSystem<TParent>(parent);

         return this;
      }

      public PlayerLoopBuilder RemoveFromSubsystem(Type parentSystemType, Type systemType) {
         _playerLoopSystem.TryFindSubSystem(out PlayerLoopSystem parent, parentSystemType);

         parent.RemoveSubSystem(systemType);

         _playerLoopSystem.ReplaceSubSystem(parent, parentSystemType);

         return this;
      }
   }
}

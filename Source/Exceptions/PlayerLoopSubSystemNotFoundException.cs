using System;

namespace Violoncello.PlayerLoopExtensions {
   public sealed class PlayerLoopSubSystemNotFoundException : Exception {
      public PlayerLoopSubSystemNotFoundException() {
         
      }

      public PlayerLoopSubSystemNotFoundException(string message) : base(message) {
         
      }

      public PlayerLoopSubSystemNotFoundException(string message, Exception innerException) : base(message, innerException) {

      }
   }
}

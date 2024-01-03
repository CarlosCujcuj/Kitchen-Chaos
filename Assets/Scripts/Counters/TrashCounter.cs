using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCounter : BaseCounter {

  public static event EventHandler OnAnyObjectTrashed;

  new public static void ResetStaticData() { // new cause BaseCounter already has that func
        // SoundManager has added suscribers to this event
        // so here we set all suscribers to null and avoid 
        // references pointing to null events
        OnAnyObjectTrashed = null;
    }

  public override void Interact(Player player) {
    if (player.HasKitchenObject()) {
        player.GetKitchenObject().DestroySelf();

        OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty);
    }
  }
}

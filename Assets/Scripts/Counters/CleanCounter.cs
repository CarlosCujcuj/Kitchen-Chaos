using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanCounter : BaseCounter, IKitchenObjectParent {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public override void Interact(Player player) {
        if (!HasKitchenObject()){
            // There is no KitchenObject here
            if (player.HasKitchenObject()){
                // Player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            } else {
                // Player not carrying anything
            }
        } else {
            // There is a KitchenObject here

            // Player is carrying something
            if (player.HasKitchenObject()){
                
                // Player is holding a Plate
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
                    
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
                        GetKitchenObject().DestroySelf();
                    }
                    
                    
                } else {
                    // player not holding Plate but something else
                    
                    // Counter is holding a Plate
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) { // Re-using previous PlateKitchenObject

                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }

            } else {
                // Player is not carrying anything
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
        
    }
}

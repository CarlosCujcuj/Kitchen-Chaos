using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour {

    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    // reference to the current parent
    private IKitchenObjectParent kitchenObjectParent;

    public KitchenObjectSO GetKitchenObjectSO() {
        return kitchenObjectSO;
    }

    // IKitchenObjectParent type cause the parent can be either a Player or a Counter
    // both extendes this Interface
    public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent) {
        if (this.kitchenObjectParent != null) {
            // goes to the current parent (reference) and 
            // sets the parent's kitchenObject (child) to null
            this.kitchenObjectParent.ClearKitchenObject();    
        }

        // sets new reference to new parent
        this.kitchenObjectParent = kitchenObjectParent;
        
        if (kitchenObjectParent.HasKitchenObject()){
            Debug.Log("IKitchenObjectParent already has a KitchenObject");
        }

        // sets parent's child (kitchenObject) as this instance
        kitchenObjectParent.SetKitchenObject(this);

        // sets current parent to the parent's 'CounterTopPoint' (BaseCounter) or 'KitchenObjectHoldPoint' (Player)
        transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
        transform.localPosition = Vector3.zero; // position inside the parent
    }

    public IKitchenObjectParent GetKitchenObjectParent() {
        return kitchenObjectParent;
    }

    public void DestroySelf() {
        kitchenObjectParent.ClearKitchenObject();

        Destroy(gameObject);
    }

    // By making it static means that this function is going to belong
    // to the class itself as opposed to any instance
    public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent) {

        // Clones the gameObject into the CounterTopPoint transform
        // Basically create the child inside the parent transform 
        // (with this you specify the position and rotation of the child that inherits from the parent)
        // Here we use Instantiate cause we cannot use the prefab directly, cause it's a prefab (template)
        Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>(); 
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);

        return kitchenObject;
    }

    public bool TryGetPlate(out PlateKitchenObject plateKitchenObject) {
        if( this is PlateKitchenObject) {
            plateKitchenObject = this as PlateKitchenObject;
            return true;
        } else {

            // Whe working with output you always need to make sure to set
            // the output before you exit the function
            // => We must assign it to something before we return 
            plateKitchenObject = null;
            return false;
        }
    }
}

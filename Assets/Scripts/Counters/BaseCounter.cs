using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent {

    public static event EventHandler OnAnyObjectPlacedHere;

    public static void ResetStaticData() {
        // SoundManager has added suscribers to this event
        // so here we set all suscribers to null and avoid 
        // references pointing to null events
        OnAnyObjectPlacedHere = null;
    }

    [SerializeField] private Transform counterTopPoint;

    private KitchenObject kitchenObject;

    // FYI
    // Basically whatever function we define with this accessor ('protected')
    // it's going to be accessible to this class and any class that extends it
    // ClearCounter can call it, but Player no

    // virtual: for every function that we want the child classes to be able
    // to implement in their own way, we can define it as virtual
    // abstract: basically forces all the classes to implement this function in their own way (kind of like an Interface)
    public virtual void Interact(Player player) {
        Debug.LogError("BaseCounter.Interact()");
    }

    public virtual void InteractAlternate(Player player) {
        // Debug.LogError("BaseCounter.InteractAlternate()");
    }


    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject;
    }
}

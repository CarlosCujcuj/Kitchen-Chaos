using System; // included to use EventHandler class
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent {

    //Properties
    // Basically is the same as a regular variable except
    // that you can add some logic when getting and setting that field
    public static Player Instance { get; private set; }

    /* == the code above is a shortcut of this one ==
    private static Player instance;
    public static Player Instance {
        get { return instance; }
        set { instance = value; }
    }*/

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged; // Generics used
    public class OnSelectedCounterChangedEventArgs : EventArgs { // created to store the Args of the EventHandler
        public BaseCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking;
    private Vector3 lastInteractDir;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this; // Singleton Pattern
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction; // suscriber added here to GameInput publisher
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAlternateAction(object sender, System.EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;

        if (selectedCounter != null) {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        
        if (selectedCounter != null) {
            selectedCounter.Interact(this);
        }
    }
 
    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking(){
        return isWalking;
    }


    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, counterLayerMask)) {
            if(raycastHit.transform.TryGetComponent(out BaseCounter baseCounter)){
                // Has ClearCounter
                if (baseCounter != selectedCounter){ // if this counter already was selected
                    SetSelectedCounter(baseCounter);
                }
            } else {
                SetSelectedCounter(null);
            }
        } else {
            SetSelectedCounter(null);
        }
    }


    private void SetSelectedCounter(BaseCounter selectedCounter){
        this.selectedCounter = selectedCounter; 

        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
            selectedCounter = selectedCounter
        });
    }


    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight , playerRadius, moveDir, moveDistance);

        if (!canMove) {
            // Cannot move towards moveDir

            // Attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized; // to always have the same magnitud in all direction
            canMove = (moveDir.x < -.5f || moveDir.x > +.5F) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight , playerRadius, moveDirX, moveDistance);

            if (canMove) {
                // Can move only on the X
                moveDir = moveDirX;
            } else {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized; // to always have the same magnitud in all direction
                canMove = (moveDir.z < -.5f || moveDir.z > +.5F) && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight , playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    moveDir = moveDirZ;
                }
                //else => cannot move in any direction
            }
        }
        
        if (canMove) {
            // (Vector3)inputVector -> Will cast a Vector2 into a Vector3
            transform.position += moveDir * moveDistance;
        }
        

        // Lerp: useful if you are dealing with just position 
        // Slerp: interpolates but with a spherical interpolation
        //    useful if you're dealing with rotation on the same spot

        isWalking = moveDir != Vector3.zero; // Vector3.zero means that he's not facing any direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed); // *Splines would be useful to understand Slerp
        
        // .forward *makes the the player face to the correct direction
        // Other methods could be:
        //  .roation     *expects a Quaternion
        //  .eulerAngles *expects Vector3
        //  .LookAt      *expects Vector3 
    }



    public Transform GetKitchenObjectFollowTransform() {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
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

using System;
using UnityEngine;
using UnityEngine.EventSystems;



public class Player : MonoBehaviour
{
    public static Player Instance{get; private set; }

    public event EventHandler <OnSelectedCounterChangedEventArgs>OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs {
        public ClearCounter selectedCounter;
    }

    [SerializeField]private float moveSpeed = 7f;
    [SerializeField]private GameInput gameInput;
    [SerializeField]private LayerMask countersLayerMask;
    

    private bool isWalking;
    private Vector3 lastInteractDir;
    private ClearCounter selectedCounter;


    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }
    private void GameInput_OnInteractAction(object sender , System.EventArgs e){
        
            if (selectedCounter != null)    {
                selectedCounter.Interact();
        }
        
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("there is more than one player sinstance");
        }
        Instance = this;
    }
    private void Update()
    {
        HandleMovement();
        HandleInteractions();
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if ( moveDir != Vector3.zero){
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                //has Clearcounter
                //clearCounter.Interact();
                if (clearCounter != selectedCounter) {
                    SetSelectedCounter(clearCounter);

                    //OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
                    //{
                    //    selectedCounter = selectedCounter
                    //});
                }
            } else {
                SetSelectedCounter(null);
                //OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
                //{
                //    selectedCounter = selectedCounter
                //});
            }
        }else{
            SetSelectedCounter(null);
            //OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
            //{
            //    selectedCounter = selectedCounter
            //});
        }
        Debug.Log(selectedCounter);       
    }
    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerRadius = .7f;

        float playerHeight = 2f;

        float moveDistance = moveSpeed * Time.deltaTime;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //attempt only X Movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                //can move only on the X
                moveDir = moveDirX;
                //transform.position += moveDir * moveDistance;
            }
            else
            {
                //cannot move only on the X
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    //can move only on the z
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }
            }

        }
        if (canMove)
        {

            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        //this code below smoothes the player rotation that's why we've user Slerp.
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }
    private void SetSelectedCounter(ClearCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });

    }
}

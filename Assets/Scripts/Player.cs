using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public ClearCounter selectedCounter;
    }

    public static Player Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotateSpeed = 15f;
    [SerializeField] GameInput gameInput;
    [SerializeField] LayerMask countersLayerMask;
    [SerializeField] Transform kitchenObjectHoldPoint;

    private bool isWalking = false;
    public bool IsWalking
    {
        get { return isWalking; }
    }

    private ClearCounter selectedCounter;

    private Vector3 lastInteractDir = Vector3.zero;

    private KitchenObject kitchenObject;

    void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;   
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleInteraction();
    }

    void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        float moveDistance = moveSpeed * Time.deltaTime;

        float playerRadius = 0.7f;
        float playerHeight = 2f;

        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            // 움직일 수 없는 경우

            // X축방향 또는 Z축 방향으로 이동 할 수 있는지를 따로 체크
            // x축
            Vector3 moveDirX = new Vector3(moveDir.x, 0 , 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else 
            {
                //Z축
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }
        }
        
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir.magnitude > float.Epsilon;

        if(isWalking)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveDir, rotateSpeed * Time.deltaTime);
        }
    }

    void HandleInteraction()
    {
        ClearCounter currentSelectedCounter = null;
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y).normalized;

        if(moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactionDistance = 2f;

        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactionDistance, countersLayerMask))
        {
            if(raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
            {
                currentSelectedCounter = clearCounter;
            }
        }

        if (currentSelectedCounter != selectedCounter)
        {
            selectedCounter = currentSelectedCounter;
            OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs {
                selectedCounter = selectedCounter
            });
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}

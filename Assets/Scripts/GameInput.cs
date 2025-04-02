using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionAsset;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;

    InputAction moveAction;
    InputAction interactAction;
    InputAction interactAlternateAction;

    void Awake()
    {
        inputActionAsset.Enable();
        moveAction = inputActionAsset["Player/Move"];
        interactAction = inputActionAsset["Player/Interact"];
        interactAlternateAction = inputActionAsset["Player/InteractAlternate"];
        
        interactAction.performed += Interact_performed;
        interactAlternateAction.performed += InteractAlternate_performed;
    }

    // Update is called once per frame
    public Vector3 GetMovementVectorNormalized()
    {
        Vector2 inputDir = moveAction.ReadValue<Vector2>();
        return inputDir.normalized;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void InteractAlternate_performed(InputAction.CallbackContext context)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
}

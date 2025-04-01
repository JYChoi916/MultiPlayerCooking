using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActionAsset;
    public event EventHandler OnInteractAction;

    InputAction moveAction;
    InputAction interactionAction;

    void Awake()
    {
        inputActionAsset.Enable();
        moveAction = inputActionAsset["Player/Move"];
        interactionAction = inputActionAsset["Player/Interact"];
        interactionAction.performed += Interact_performed;
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
}

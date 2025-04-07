using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; } // Singleton instance

    [SerializeField] private InputActionAsset inputActionAsset;
    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseAction; // Pause action event

    InputAction moveAction;
    InputAction interactAction;
    InputAction interactAlternateAction;
    InputAction pauseAction; // Pause action

    void Awake()
    {
        Instance = this;
        inputActionAsset.Enable();
        moveAction = inputActionAsset["Player/Move"];
        interactAction = inputActionAsset["Player/Interact"];
        interactAlternateAction = inputActionAsset["Player/InteractAlternate"];
        pauseAction = inputActionAsset["Player/Pause"];
        
        interactAction.performed += Interact_performed;
        interactAlternateAction.performed += InteractAlternate_performed;
        pauseAction.performed += Pause_performed; // Subscribe to pause action
    }

    void OnDestroy()
    {
        interactAction.performed -= Interact_performed;
        interactAlternateAction.performed -= InteractAlternate_performed;
        pauseAction.performed -= Pause_performed; // Subscribe to pause action

        moveAction.Dispose();
        interactAction.Dispose();
        interactAlternateAction.Dispose();
        pauseAction.Dispose();

        inputActionAsset.Disable();
    }

    // Update is called once per frame
    public Vector3 GetMovementVectorNormalized()
    {
        Vector2 inputDir = moveAction.ReadValue<Vector2>();
        return inputDir.normalized;
    }

    private void Interact_performed(InputAction.CallbackContext obj)
    {
        if (KitchenGameManager.Instance.IsGamePlaying() == false) 
            return; // 게임이 진행 중이지 않으면 인터랙트 무시

        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void InteractAlternate_performed(InputAction.CallbackContext context)
    {
        if (KitchenGameManager.Instance.IsGamePlaying() == false) 
            return; // 게임이 진행 중이지 않으면 인터랙트 무시

        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }
    
    private void Pause_performed(InputAction.CallbackContext context)
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty); // Pause action event
    }
}

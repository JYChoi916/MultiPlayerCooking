using System;
using UnityEditor;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    public static void ResetStaticData() {
        OnAnyObjectPlacedHere = null;
    }

    public static event EventHandler OnAnyObjectPlacedHere;

    [SerializeField] protected Transform counterTopPoint;

    protected KitchenObject kitchenObject;

    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null) 
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty); // 오브젝트가 카운터에 놓일 때 이벤트 발생
        }
    }

    public KitchenObject GetKitchenObject() {
        return kitchenObject;
    }

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() {
        return kitchenObject != null;
    }

    public virtual void Interact(Player player) {}
    public virtual void InteractAlternate(Player player) {}
}

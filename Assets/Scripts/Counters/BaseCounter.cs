using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
    [SerializeField] protected Transform counterTopPoint;
    [SerializeField] protected KitchenObjectSO kitchenObjectSO;
    protected KitchenObject kitchenObject;

    public Transform GetKitchenObjectFollowTransform() {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
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
}

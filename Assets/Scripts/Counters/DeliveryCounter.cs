using UnityEngine;

public class DeliveryCounter : BaseCounter
{
    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            // 플레이어가 오브젝트를 가지고 있고
            if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
            {
                // 플레이어의 오브젝트가 PlateKitchenObject인 경우
                DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
                plateKitchenObject.DestroySelf(); // 접시 제거
            }
        }
    }
}

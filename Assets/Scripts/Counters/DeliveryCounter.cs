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
                if(DeliveryManager.Instance.DeliverRecipe(plateKitchenObject)) // 서빙 실행
                {
                    // 서빙 성공
                }
                else
                {
                    // 서빙 실패
                }

                plateKitchenObject.DestroySelf(); // 접시 제거
            }
        }
    }
}

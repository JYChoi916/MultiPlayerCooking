using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    public override void Interact(Player player) 
    {
        if (!HasKitchenObject())
        {
            // 이 카운터가 주방 오브젝트를 가지고 있지 않다면
            if (player.HasKitchenObject())
            {
                // 플레이어가 주방 오브젝트를 가지고 있다면

                // 플레이어가 가지고 있는 주방 오브젝트를 이 카운터의 자식으로 설정
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                // 가지고 있지 않다면
            }
        }
        else {
            // 이 카운터가 오브젝트를 가지고 있다면
            if (player.HasKitchenObject())
            {
                // 플레이어가 오브젝트를 가지고 있다면
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // 플레이어가 접시를 가지고 있다면
                    // 접시에 올려보고
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // 제대로 접시에 올려졌다면
                        GetKitchenObject().DestroySelf(); // 카운터에 있는 주방 오브젝트를 파괴
                    }
                }
                else
                {
                    // 플레이어가 들고 있는게 접시가 아니다!

                    // 그럼 카운터에 있는 오브젝트는 접시인가?
                    if (GetKitchenObject().TryGetPlate(out plateKitchenObject))
                    {
                        // 접시가 맞다면
                        // 플레이어가 가지고 있는 오브젝트를 접시에 올려보고
                        if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            // 제대로 접시에 올려졌다면
                            // 플레이어가 가지고 있는 오브젝트를 파괴
                            player.GetKitchenObject().DestroySelf();
                        }
                    }
                }
            }
            else 
            {
                // 가지고 있지 않다면
                // 이 카운터에 있는 주방 오브젝트를 플레이어의 자식으로 설정
                // (즉, 플레이어가 주방 오브젝트를 가져가는 것)
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}

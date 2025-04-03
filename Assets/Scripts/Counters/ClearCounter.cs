using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    public override void Interact(Player player) 
    {
        // 만일 플레이어가 주방 오브젝트를 가지고 있고, 이 카운터가 주방 오브젝트를 가지고 있지 않다면
        if (player.HasKitchenObject() && !HasKitchenObject())
        {
            // 플레이어가 가지고 있는 주방 오브젝트를 이 카운터의 자식으로 설정
            // (즉, 이 카운터에 주방 오브젝트를 올려놓는 것)
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else {
            // 만일 플레이어가 주방 오브젝트를 가지고 있지 않고, 이 카운터가 주방 오브젝트를 가지고 있다면
            // (즉, 이 카운터에 주방 오브젝트가 올려져 있다면)
            if (!player.HasKitchenObject() && HasKitchenObject())
            {
                // 이 카운터에 있는 주방 오브젝트를 플레이어의 자식으로 설정
                // (즉, 플레이어가 주방 오브젝트를 가져가는 것)
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }
}

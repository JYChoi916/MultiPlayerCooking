using System;
using Unity.VisualScripting;
using UnityEngine;

public class TrashCounter : BaseCounter
{
    new public static void ResetStaticData() {
        OnAnyObjectTrashed = null;
    }

    public static event EventHandler OnAnyObjectTrashed;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject())
        {
            player.GetKitchenObject().DestroySelf();
            OnAnyObjectTrashed?.Invoke(this, EventArgs.Empty); // 오브젝트가 쓰레기통에 버려질 때 이벤트 발생
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter, IHasProgress
{
    new public static void ResetStaticData()
    {
        OnAnyCut = null;        
    }

    public static event EventHandler OnAnyCut;

    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress = 0;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            // 플레이어가 손에 오브젝트를 들고 있지 않은 경우
            if (player.HasKitchenObject())
            {
                // 이 카운터가 오브젝트를 가지고 있지 않다면
                player.GetKitchenObject().SetKitchenObjectParent(this); // 플레이어가 가지고 있는 주방 오브젝트를 이 카운터의 자식으로 설정
            }
            else 
            {
                // 플레이어가 오브젝트를 가지고 있지 않다면
            }
        }
        else {
            // 이 카운터가 오브젝트를 가지고 있다면
            if (player.HasKitchenObject())
            {
                // 플레이어가 오브젝트를 가지고 있다면
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // 플레이어가 접시를 가지고 있다!
                    // 접시에 올려 보고
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        // 제대로 접시에 올려졌다면
                        GetKitchenObject().DestroySelf();// 카운터에 있는 오브젝트를 파괴
                    }
                }
                else
                {
                    // 플레이어어가 들고 있는게 접시가 아니다!

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
                // 플레이어가 오브젝트를 가지고 있지 않다면
                CuttingRecipeSO correctRecipeSO = GetCorrectCuttingRecipe(GetKitchenObject().GetKitchenObjectSO());            
                bool isCutting = (cuttingProgress > 0 && cuttingProgress < correctRecipeSO.cuttingProgressMax);
                if (!player.HasKitchenObject() && HasKitchenObject() && isCutting == false)
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                    cuttingProgress = 0;
                }
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            CuttingRecipeSO correctRecipeSO = GetCorrectCuttingRecipe(GetKitchenObject().GetKitchenObjectSO());
            
            if (correctRecipeSO != null)
            {
                cuttingProgress++;

                OnAnyCut?.Invoke(this, EventArgs.Empty); // 컷팅 이벤트 발생

                if (cuttingProgress >= correctRecipeSO.cuttingProgressMax)
                {
                    cuttingProgress = 0;
                    GetKitchenObject().DestroySelf();
                    KitchenObject.SpawnKitchenObject(correctRecipeSO.output, this);
                }

                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs {
                    progressAmount = (float)cuttingProgress / correctRecipeSO.cuttingProgressMax
                });
            }
        }
    }

    private CuttingRecipeSO GetCorrectCuttingRecipe(KitchenObjectSO inputKitchenObjectSO)
    {
        foreach (CuttingRecipeSO recipeSO in cuttingRecipeSOArray)
        {
            if (recipeSO.input == inputKitchenObjectSO)
            {
                return recipeSO;
            }
        }

        return null;
    }
}

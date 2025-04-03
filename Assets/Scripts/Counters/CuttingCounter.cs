using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter, IHasProgress
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    private int cuttingProgress = 0;

    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() && !HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else {
            CuttingRecipeSO correctRecipeSO = GetCorrectCuttingRecipe(GetKitchenObject().GetKitchenObjectSO());            
            bool isCutting = (cuttingProgress > 0 && cuttingProgress < correctRecipeSO.cuttingProgressMax);
            if (!player.HasKitchenObject() && HasKitchenObject() && isCutting == false)
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                cuttingProgress = 0;
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

using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class CuttingCounter : BaseCounter
{
    [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressAmount;
    }

    private int cuttingProgress = 0;

    public override void Interact(Player player)
    {
        if (player.HasKitchenObject() && !HasKitchenObject())
        {
            player.GetKitchenObject().SetKitchenObjectParent(this);
        }
        else {
            if (!player.HasKitchenObject() && HasKitchenObject())
            {
                GetKitchenObject().SetKitchenObjectParent(player);
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

                OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs {
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

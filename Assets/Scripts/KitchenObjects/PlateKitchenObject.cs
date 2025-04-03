using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded; // 재료 추가 이벤트
    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSO; // 추가된 재료
    }

    [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList; // 접시에 담길 수 있는 재료 리스트
    private List<KitchenObjectSO> kitchenObjectSOList; // 접시에 담긴 재료 리스트

    void Awake()
    {
        kitchenObjectSOList = new List<KitchenObjectSO>(); // 재료 리스트 초기화
    }

    public bool TryAddIngredient(KitchenObjectSO ingredientSO)
    {
        if (!validKitchenObjectSOList.Contains(ingredientSO))
        {
            // 유효하지 않은 재료인 경우
            return false;
        }

        if (kitchenObjectSOList.Contains(ingredientSO))
        {
            // 이미 재료가 존재하는 경우
            return false;
        }
        else 
        {
            // 재료가 익힌고기 일 경우 또는 탄 고기일 경우
            if (ingredientSO.name.Contains("Meat"))
            {
                if (IsMeatAlreadyAdded())
                {
                    // 이미 고기가 추가된 경우
                    return false; // 재료 추가 실패
                }
            }

            kitchenObjectSOList.Add(ingredientSO); // 재료 추가
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs {
                kitchenObjectSO = ingredientSO 
            }); // 재료 추가 이벤트 발생
            return true;
        }
    }

    bool IsMeatAlreadyAdded()
    {
        foreach (var kitchenObjectSO in kitchenObjectSOList)
        {
            if (kitchenObjectSO.name.Contains("Meat"))
            {
                return true; // 고기가 이미 추가된 경우
            }
        }
        return false; // 고기가 추가되지 않은 경우
    }
}

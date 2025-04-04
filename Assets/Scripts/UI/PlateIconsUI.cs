using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlateIconsUI : MonoBehaviour
{
    [SerializeField] private PlateKitchenObject plateKitchenObject;
    [SerializeField] private GameObject iconTemplate; // 아이콘 템플릿

    void Start()
    {
        plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded; // 재료 추가 이벤트 구독
    }

    private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        AddIcon(e.kitchenObjectSO); // 재료 추가 시 UI 업데이트
    }

    private void AddIcon(KitchenObjectSO kitchenObjectSO)
    {
        GameObject iconObject = Instantiate(iconTemplate, transform); // 아이콘 템플릿 복사
        // 아이콘을 재료에 맞게 설정
        iconObject.GetComponent<PlateIconSingleUI>().SetKitchinObjectSO(kitchenObjectSO);
    }
}

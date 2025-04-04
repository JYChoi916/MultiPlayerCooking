using System;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{ 
    [SerializeField] private Transform container; // UI 컨테이너
    [SerializeField] private GameObject recipeTemplate; // 레시피 템플릿

    void Start()
    {
        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned; // 레시피 생성 이벤트 구독
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted; // 레시피 완료 이벤트 구독
    }

    private void DeliveryManager_OnRecipeSpawned(object sender, DeliveryManager.OnRecipeEventArgs e)
    {
        AddRecipeUI(e.recipeSO);
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, DeliveryManager.OnRecipeEventArgs e)
    {
        RemoveRecipeUI(e.recipeSO);
    }

    // Update is called once per frame
    void AddRecipeUI(RecipeSO recipeSO)
    {
        GameObject recipeUIObject = Instantiate(recipeTemplate, container); // 레시피 UI 템플릿 복제
        recipeUIObject.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
    }

    void RemoveRecipeUI(RecipeSO recipeSO)
    {
        foreach (Transform child in container)
        {
            if (child.GetComponent<DeliveryManagerSingleUI>().GetRecipeSO() == recipeSO)
            {
                Destroy(child.gameObject); // 레시피 UI 제거
                break; // 제거 후 반복문 종료
            }
        }
    }
}

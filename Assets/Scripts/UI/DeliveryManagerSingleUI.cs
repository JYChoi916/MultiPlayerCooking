using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText; // 레시피 이름 텍스트
    [SerializeField] private Transform iconContainer; // 아이콘 컨테이너
    [SerializeField] private GameObject iconTemplate; // 아이콘 템플릿 프리팹

    private RecipeSO recipeSO;
    public RecipeSO GetRecipeSO() => recipeSO; // recipeSO 반환

    public void SetRecipeSO(RecipeSO recipeSO)
    {
        this.recipeSO = recipeSO; // 레시피 SO 설정
        recipeNameText.text = recipeSO.recipeName; // 레시피 이름 설정
        
        foreach (KitchenObjectSO kitchenObjectSO in recipeSO.kitchenObjectSOList)
        {
            GameObject iconObject = Instantiate(iconTemplate, iconContainer); // 아이콘 템플릿 복제
            iconObject.GetComponent<Image>().sprite = kitchenObjectSO.sprite; // 아이콘 이미지 설정
        }
    }
}

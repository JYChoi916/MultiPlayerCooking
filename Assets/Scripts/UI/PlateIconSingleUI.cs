using UnityEngine;
using UnityEngine.UI;

public class PlateIconSingleUI : MonoBehaviour
{
    [SerializeField] private Image image; // 아이콘 이미지

    public void SetKitchinObjectSO(KitchenObjectSO kitchenObjectSO)
    {
        image.sprite = kitchenObjectSO.sprite; // 아이콘 이미지 설정
    }
}

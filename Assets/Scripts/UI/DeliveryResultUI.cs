using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DeliveryResultUI : MonoBehaviour
{
    private const string POP_UP = "Popup"; // 애니메이션 트리거 파라미터 이름름

    [SerializeField] private Image backgroundImage;       // 배경 이미지
    [SerializeField] private Image iconImage;             // 아이콘 이미지
    [SerializeField] private TextMeshProUGUI messageText; // 메시지 텍스트
    [SerializeField] private Color successColor;          // 성공 색상
    [SerializeField] private Color failColor;             // 실패 색상
    [SerializeField] private Sprite successSprite;        // 성공 아이콘
    [SerializeField] private Sprite failSprite;           // 실패 아이콘

    private Animator animator; // 애니메이터 컴포넌트
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted; // 레시피 완료 이벤트 구독
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed; // 레시피 실패 이벤트 구독
        gameObject.SetActive(false);
    }

    private void DeliveryManager_OnRecipeCompleted(object sender, DeliveryManager.OnRecipeEventArgs e)
    {
        gameObject.SetActive(true); // UI 활성화
        animator.SetTrigger(POP_UP); // 애니메이션 트리거 설정
        backgroundImage.color = successColor;
        iconImage.sprite = successSprite; // 성공 아이콘 설정
        messageText.text = "DELIVERY\nSUCCESS";
    }

    private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
    {
        gameObject.SetActive(true); // UI 활성화
        animator.SetTrigger(POP_UP); // 애니메이션 트리거 설정
        backgroundImage.color = failColor;
        iconImage.sprite = failSprite; // 실패 아이콘 설정
        messageText.text = "DELIVERY\nFAILED";
    }
}

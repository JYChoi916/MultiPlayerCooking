using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText; // 카운트 텍스트
    [SerializeField] private Button mainMenuButton; // 메인 메뉴 버튼

    void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged; // 게임 매니저 상태 변경 이벤트 구독
        mainMenuButton.onClick.AddListener(() => {
            Time.timeScale = 1f; // 게임 속도 초기화
            Loader.Load(Loader.Scene.MainMenuScene); // 메인 메뉴 씬 로드
        });
        gameObject.SetActive(false); // 초기에는 비활성화
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            countText.text = DeliveryManager.Instance.GetSuccessfulRecipeCount().ToString(); // 성공적인 레시피 개수 표시
        }
        gameObject.SetActive(KitchenGameManager.Instance.IsGameOver());
    }
}

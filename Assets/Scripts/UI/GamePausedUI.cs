using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;

    void Start()
    {
        KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused; // 게임 일시 정지 이벤트 구독
        KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused; // 게임 재개 이벤트 구독

        resumeButton.onClick.AddListener(() => {
            KitchenGameManager.Instance.TogglePauseGame();
        });

        mainMenuButton.onClick.AddListener(() => {
            Time.timeScale = 1f;
            Loader.Load(Loader.Scene.MainMenuScene);
        });
        gameObject.SetActive(false); // 초기에는 비활성화
    }

    private void KitchenGameManager_OnGamePaused(object sender, EventArgs e)
    {
        gameObject.SetActive(true);
    }

    private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
    {
        gameObject.SetActive(false);
    }
}

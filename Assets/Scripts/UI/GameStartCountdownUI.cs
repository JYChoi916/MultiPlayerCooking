using System;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class GameStartCountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText; // 카운트다운 텍스트

    void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += KitchenGameManager_OnStateChanged; // 게임 매니저 상태 변경 이벤트 구독
    }

    void Update()
    {
        countdownText.text = KitchenGameManager.Instance.GetCountdownToStartTimer().ToString("0");
    }

    private void KitchenGameManager_OnStateChanged(object sender, EventArgs e)
    {
        gameObject.SetActive(KitchenGameManager.Instance.IsCountdownToStart());
    }
}

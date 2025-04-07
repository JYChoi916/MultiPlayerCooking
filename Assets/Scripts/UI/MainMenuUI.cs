using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton; // 플레이 버튼
    [SerializeField] private Button quitButton; // 종료 버튼

    private void Awake()
    {
        playButton.onClick.AddListener( () => {
            Loader.Load(Loader.Scene.GameScene);
        }); // 플레이 버튼 클릭 이벤트 등록

        quitButton.onClick.AddListener( () => {
            Application.Quit(); // 종료 버튼 클릭 시 애플리케이션 종료
            Debug.Log("Application.Quit()"); // 디버그 로그 출력
        }); // 종료 버튼 클릭 이벤트 등록
    }

}

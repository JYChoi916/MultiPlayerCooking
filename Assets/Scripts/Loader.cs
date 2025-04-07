using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }

    public static Scene targetScene; // 로드할 씬

    public static void Load(Scene targetScene) 
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString()); // 로딩 씬으로 전환
    }

    public static void LoadCallback() 
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}

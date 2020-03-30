using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScene : MonoSingleton
{
    public const string NAME = "Menu";
	public GameObject playButton;

	public static MenuScene instance
	{
		get
		{
			return GetInstance<MenuScene>();
		}
	}

	void Start()
    {
		InitScene.instance.loadingPanel.Exit();
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuScene.NAME));

		GameManager.instance.LoadLocalPlayer();
	}

	public void OnPlayClick()
    {
		Debug.Log("OnPlayClick()");
		ScenesFlowManager.instance.UnloadingMenuScene();
	}

	public void DestroyScene()
	{
		Destroy(gameObject);
		Resources.UnloadUnusedAssets();
	}
}

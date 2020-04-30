using UnityEngine;
using UnityEngine.UI;

public class MainScene : MonoSingleton
{
	public const string NAME = "Main";

	public Canvas uiWorldCanvas;
	public SuggestionMenu suggestionMenu;
	public MainSceneChef mainSceneChef;
	public Image backgroundSkyImage;
	public Color skyDayColor;

	private MainSceneFsm sceneFsm;

	public static MainScene instance
	{
		get
		{
			return GetInstance<MainScene>();
		}
	}

	public void Init()
	{
		sceneFsm = new MainSceneFsm(this);
		sceneFsm.StartFsm();
	}

	protected override void OnMonoSingletonUpdate()
	{
		if (sceneFsm != null)
		{
			sceneFsm.Update();
		}
	}

	public void DestroyScene()
	{
		Hud.instance.DestroyHud();
		Destroy(gameObject);
		Resources.UnloadUnusedAssets();
	}

	public void SetupScene()
	{
		Hud.instance.Setup();
		SetSkyDayColor();
		InitScene.instance.loadingPanel.Exit(OnLoadingPanelExitCompleted);
	}

	public void UnsetupScene()
	{
		Hud.instance.Unsetup();
	}

	public void SetSkyDayColor()
	{
		backgroundSkyImage.color = skyDayColor;
	}

	private void OnLoadingPanelExitCompleted()
	{
		sceneFsm.TriggerState(MainSceneFsm.PlayState);
	}
}

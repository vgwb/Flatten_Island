using UnityEngine;

public class MainScene : MonoSingleton
{
	public const string NAME = "Main";

	public Canvas uiWorldCanvas;
	public SuggestionMenu suggestionMenu;

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
		InitScene.instance.loadingPanel.Exit(OnLoadingPanelExitCompleted);
	}

	public void UnsetupScene()
	{
		Hud.instance.Unsetup();
	}

	private void OnLoadingPanelExitCompleted()
	{
		sceneFsm.TriggerState(MainSceneFsm.PlayState);
	}
}

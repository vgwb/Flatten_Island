using UnityEngine.SceneManagement;

public class ScenesFlowFsm : FiniteStateMachine
{
	public static readonly string InitSceneState = "InitScene";
	public static readonly string LoadingLogoSceneState = "LoadingLogoScene";
	public static readonly string LogoSceneState = "LogoScene";
	public static readonly string UnloadingLogoSceneState = "UnloadingLogoScene";
	public static readonly string LoadingMenuSceneState = "LoadingMenuScene";
	public static readonly string MenuSceneState = "MenuScene";
	public static readonly string UnloadingMenuSceneState = "UnloadingMenuScene";
	public static readonly string LoadingMainSceneState = "LoadingMainScene";
	public static readonly string MainSceneState = "MainScene";
	public static readonly string UnloadingMainSceneState = "UnloadingMainScene";


	private ScenesFlowManager scenesFlowManager;

	public ScenesFlowFsm(ScenesFlowManager scenesFlowManager) : base()
	{
		this.scenesFlowManager = scenesFlowManager;
	}

	protected override void Initialize()
	{
		base.Initialize();
		AddState(InitSceneState, OnInitScene_OnEnter, null, null);

		AddState(LoadingLogoSceneState, LoadingLogoScene_OnEnter, LoadingLogoScene_OnUpdate, null);
		AddState(LogoSceneState, LogoScene_OnEnter, LogoScene_OnUpdate, null);
		AddState(UnloadingLogoSceneState, UnloadingLogoScene_OnEnter, UnloadingLogoScene_OnUpdate, UnloadingLogoSceneOn_OnExit);

		AddState(LoadingMenuSceneState, LoadingMenuScene_OnEnter, LoadingMenuScene_OnUpdate, null);
		AddState(MenuSceneState, MenuScene_OnEnter, null, null);
		AddState(UnloadingMenuSceneState, UnloadingMenuScene_OnEnter, UnloadingMenuScene_OnUpdate, UnloadingMenuSceneOn_OnExit);

		AddState(LoadingMainSceneState, LoadingMainScene_OnEnter, LoadingMainScene_OnUpdate, null);
		AddState(MainSceneState, MainScene_OnEnter, null, null);
		AddState(UnloadingMainSceneState, UnloadingMainScene_OnEnter, UnloadingMainScene_OnUpdate, UnloadingMainSceneOn_OnExit);
	}

	public void StartFsm()
	{
		Start(InitSceneState);
	}

	private void OnInitScene_OnEnter()
	{
#if CHEAT_DEBUG
		if (CheatManager.instance.skipLogo)
		{
			if (GameManager.instance.HasPendingGameSession())
			{
				TriggerState(LoadingMainSceneState);
			}
			else
			{
				TriggerState(LoadingMenuSceneState);
			}
		}
		else
		{
			TriggerState(LoadingLogoSceneState);
		}
		return;
#else
		TriggerState(LoadingLogoSceneState);
#endif
	}

	private void LoadingLogoScene_OnEnter()
	{
		scenesFlowManager.loadingAsyncOperation = SceneManager.LoadSceneAsync(global::LogoScene.NAME, LoadSceneMode.Additive);
	}

	private void LoadingLogoScene_OnUpdate()
	{
		if (scenesFlowManager.loadingAsyncOperation.isDone)
		{
			TriggerState(LogoSceneState);
		}
	}

	private void LogoScene_OnEnter()
	{
		LogoScene.instance.logoSceneChef.Cook(LogoScene.instance.logoSceneChef.enterLogoRecipe, OnEnterLogoRecipeCompleted);
	}

	private void OnEnterLogoRecipeCompleted()
	{
		TriggerState(UnloadingLogoSceneState);
	}

	private void LogoScene_OnUpdate()
	{
	}

	private void UnloadingLogoScene_OnEnter()
	{
		InitScene.instance.loadingPanel.Enter(OnEnterLogoSceneLoadingPanelCompleted);
	}

	private void OnEnterLogoSceneLoadingPanelCompleted()
	{
		LogoScene.instance.DestroyScene();
		scenesFlowManager.unloadingAsyncOperation = SceneManager.UnloadSceneAsync(global::LogoScene.NAME);
	}

	private void UnloadingLogoScene_OnUpdate()
	{
		if (scenesFlowManager.unloadingAsyncOperation!= null && scenesFlowManager.unloadingAsyncOperation.isDone)
		{
			if (GameManager.instance.HasPendingGameSession())
			{
				TriggerState(LoadingMainSceneState);
			}
			else
			{
				TriggerState(LoadingMenuSceneState);
			}
		}
	}

	private void UnloadingLogoSceneOn_OnExit()
	{
		scenesFlowManager.loadingAsyncOperation = null;
		scenesFlowManager.unloadingAsyncOperation = null;
	}

	private void LoadingMenuScene_OnEnter()
	{
		scenesFlowManager.loadingAsyncOperation = SceneManager.LoadSceneAsync(global::MenuScene.NAME, LoadSceneMode.Additive);
	}

	private void LoadingMenuScene_OnUpdate()
	{
		if (scenesFlowManager.loadingAsyncOperation.isDone)
		{
			TriggerState(MenuSceneState);
		}
	}

	private void MenuScene_OnEnter()
	{
		MenuScene.instance.Init();
	}

	private void UnloadingMenuScene_OnEnter()
	{
		InitScene.instance.loadingPanel.Enter(OnEnterMenuSceneLoadingPanelCompleted);
	}

	private void OnEnterMenuSceneLoadingPanelCompleted()
	{
		MenuScene.instance.playButton.SetActive(false);
		MenuScene.instance.DestroyScene();
		scenesFlowManager.unloadingAsyncOperation = SceneManager.UnloadSceneAsync(global::MenuScene.NAME);
	}

	private void UnloadingMenuScene_OnUpdate()
	{
		if (scenesFlowManager.unloadingAsyncOperation != null && scenesFlowManager.unloadingAsyncOperation.isDone)
		{
			TriggerState(LoadingMainSceneState);
		}
	}

	private void UnloadingMenuSceneOn_OnExit()
	{
		scenesFlowManager.loadingAsyncOperation = null;
		scenesFlowManager.unloadingAsyncOperation = null;
	}

	private void LoadingMainScene_OnEnter()
	{
		scenesFlowManager.loadingAsyncOperation = SceneManager.LoadSceneAsync(global::MainScene.NAME, LoadSceneMode.Additive);
	}

	private void LoadingMainScene_OnUpdate()
	{
		if (scenesFlowManager.loadingAsyncOperation.isDone)
		{
			TriggerState(MainSceneState);
		}
	}

	private void MainScene_OnEnter()
	{
		MainScene.instance.Init();
	}

	private void UnloadingMainScene_OnEnter()
	{
		Hud.instance.Unsetup();
		InitScene.instance.loadingPanel.Enter(OnUnloadingMainSceneLoadingPanelCompleted);
	}

	private void OnUnloadingMainSceneLoadingPanelCompleted()
	{
		MainScene.instance.UnsetupScene();
		MainScene.instance.DestroyScene();
		scenesFlowManager.unloadingAsyncOperation = SceneManager.UnloadSceneAsync(global::MainScene.NAME);
	}

	private void UnloadingMainScene_OnUpdate()
	{
		if (scenesFlowManager.unloadingAsyncOperation != null && scenesFlowManager.unloadingAsyncOperation.isDone)
		{
			TriggerState(LoadingMenuSceneState);
		}
	}

	private void UnloadingMainSceneOn_OnExit()
	{
		scenesFlowManager.loadingAsyncOperation = null;
		scenesFlowManager.unloadingAsyncOperation = null;
	}
}

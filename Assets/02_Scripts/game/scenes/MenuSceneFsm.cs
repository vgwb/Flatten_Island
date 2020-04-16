using UnityEngine.SceneManagement;

public class MenuSceneFsm : FiniteStateMachine
{
	public static readonly string InitState = "Init";
	public static readonly string ReadyState = "Ready";
	public static readonly string IntroState = "Intro";
	public static readonly string MenuState = "Menu";
	public static readonly string UninitState = "Uninit";

	private MenuScene menuScene;

	public MenuSceneFsm(MenuScene menuScene) : base()
    {
		this.menuScene = menuScene;
    }

	protected override void Initialize()
	{
		base.Initialize();
		AddState(InitState, null, Init_Update, Init_Exit);
		AddState(ReadyState, Ready_Enter, Ready_Update, null);
		AddState(IntroState, Intro_Enter, Intro_Update, Intro_Exit);
		AddState(MenuState, Menu_Enter, Menu_Update, Menu_Exit);
		AddState(UninitState, Uninit_Enter, null, null);
	}

	public void StartFsm()
	{
		Start(InitState);
	}

	private void Init_Update()
	{
		TriggerState(ReadyState);
	}

	private void Init_Exit()
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(MenuScene.NAME));
	}

	private void Ready_Enter()
    {
		menuScene.SetupScene();
	}

	private void Ready_Update()
	{
	}

	private void Intro_Enter()
	{
		menuScene.cinematicCanvas.SetActive(true);
		menuScene.introCinematicMenu.PlayCinematic();
	}

	private void Intro_Update()
	{
	}

	private void Intro_Exit()
	{
		menuScene.cinematicCanvas.SetActive(false);
	}

	private void Menu_Enter()
	{
		menuScene.menuCanvas.SetActive(true);
	}

	private void Menu_Update()
	{
	}

	private void Menu_Exit()
	{
		AudioManager.instance.StopMusic();
		menuScene.menuCanvas.SetActive(false);
	}

	private void Uninit_Enter()
	{
		ScenesFlowManager.instance.UnloadingMenuScene(); // PABLO: what if we had 2 navigations?
	}
}

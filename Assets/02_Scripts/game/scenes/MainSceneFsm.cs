using UnityEngine.SceneManagement;

public class MainSceneFsm : FiniteStateMachine
{
	public static readonly string InitState = "Init";
	public static readonly string ReadyState = "Ready";
	public static readonly string PlayState = "Play";
	public static readonly string UninitState = "Uninit";

	private MainScene mainScene;

	public MainSceneFsm(MainScene mainScene) : base()
    {
		this.mainScene = mainScene;
    }

	protected override void Initialize()
	{
		base.Initialize();
		AddState(InitState, null, Init_Update, Init_Exit);
		AddState(ReadyState, Ready_Enter, Ready_Update, null);
		AddState(PlayState, Play_Enter, Play_Update, Play_Exit);
		AddState(UninitState, null, null, null);
	}

	public void StartFsm()
	{
		Start(InitState);
	}

	private void Init_Exit()
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(MainScene.NAME));
	}

	private void Init_Update()
	{
		TriggerState(ReadyState);
	}

	private void Ready_Enter()
    {
		mainScene.SetupScene();
    }

	private void Ready_Update()
	{
		TriggerState(PlayState);
	}

	private void Play_Enter()
	{
		LocalPlayer localPlayer = GameManager.instance.localPlayer;

		if (!localPlayer.HasSession())
		{
			localPlayer.StartNewGameSession();
		}
		else
		{
			localPlayer.ResumeGameSession();
		}
		ChartManager.instance.FirstDraw();
	}

	private void Play_Update()
	{
		if (GameManager.instance.localPlayer.HasSession())
		{
			GameManager.instance.localPlayer.gameSession.UpdateFsm();
		}
	}

	private void Play_Exit()
	{
		mainScene.UnsetupScene();
	}
}

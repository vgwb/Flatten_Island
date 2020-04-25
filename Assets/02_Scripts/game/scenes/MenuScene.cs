using UnityEngine;
using UnityEngine.SceneManagement;
using Messages;

public class MenuScene : MonoSingleton
{
    public const string NAME = "Menu";
	public GameObject playButton;
	public SettingsMenu settingsMenu;
	public LanguageMenu languageMenu;
	public GameObject blockingBackground;

	public CinematicMenu introCinematicMenu;
	public GameObject menuCanvas;
	public GameObject cinematicCanvas;
	public AudioClip menuMusic;
	public HighScoreText highScoreText;


	public static MenuScene instance
	{
		get
		{
			return GetInstance<MenuScene>();
		}
	}

	private MenuSceneFsm sceneFsm;

	public void Init()
	{
		sceneFsm = new MenuSceneFsm(this);
		sceneFsm.StartFsm();
	}

	protected override void OnMonoSingletonUpdate()
	{
		if (sceneFsm != null)
		{
			sceneFsm.Update();
		}
	}

	public void UpdateBlockingBackground()
	{
		bool isSettingsGridShown = settingsMenu.IsGridShown();
		bool isLanguageGridShown = languageMenu.IsGridShown();

		bool isBlockingBackgroundActive = isSettingsGridShown || isLanguageGridShown;

		blockingBackground.SetActive(isBlockingBackgroundActive);
	}

	void Start()
    {
		EventMessageHandler playCinematicCompletedMessageHandler = new EventMessageHandler(this, OnPlayCinematicCompleted);
		EventMessageManager.instance.AddHandler(typeof(PlayCinematicCompletedEvent).Name, playCinematicCompletedMessageHandler);
	}

	private void OnPlayCinematicCompleted(EventMessage eventMessage)
	{
		PlayCinematicCompletedEvent playCinematicCompletedEvent = eventMessage.eventObject as PlayCinematicCompletedEvent;
		if (playCinematicCompletedEvent.cinematicMenu == introCinematicMenu)
		{
			GameManager.instance.localPlayer.playerSettings.skipIntro = true;
			GameManager.instance.SavePlayer();

			sceneFsm.TriggerState(MenuSceneFsm.UninitState);
		}
	}

	public void OnPlayClick()
    {
#if CHEAT_DEBUG
		if (CheatManager.instance.forceIntro)
		{
			sceneFsm.TriggerState(MenuSceneFsm.IntroState);
			return;
		}
#endif

		if (GameManager.instance.localPlayer.playerSettings.skipIntro)
		{
			sceneFsm.TriggerState(MenuSceneFsm.UninitState);
		}
		else
		{
			sceneFsm.TriggerState(MenuSceneFsm.IntroState);
		}
	}

	public void SetupScene()
	{
		menuCanvas.SetActive(false);
		cinematicCanvas.SetActive(false);
		TryDisplayHighScoreText();
		InitScene.instance.loadingPanel.Exit(OnLoadingPanelExitCompleted);
	}

	private void TryDisplayHighScoreText()
	{
		HighScore highScore = GameManager.instance.localPlayer.highScore;

		if (highScore != null && highScore.HasDayHighScore())
		{
			highScoreText.gameObject.SetActive(true);
			highScoreText.DisplayDayHighScore(highScore.day);
		}
		else
		{
			highScoreText.gameObject.SetActive(false);
		}
	}

	private void OnLoadingPanelExitCompleted()
	{
		AudioManager.instance.PlayMusic(menuMusic);
		sceneFsm.TriggerState(MenuSceneFsm.MenuState);
	}

	public void DestroyScene()
	{
		EventMessageManager.instance.RemoveHandler(typeof(PlayCinematicCompletedEvent).Name, this);

		Destroy(gameObject);
		Resources.UnloadUnusedAssets();
	}
}

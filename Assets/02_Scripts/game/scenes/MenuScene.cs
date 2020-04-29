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
	public GameObject creditsCanvas;
	public CreditsMenu creditsMenu;
	public AudioClip menuMusic;
	public HighScoreText highScoreText;
	public GameObject highScoreGroup;


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
			RegisterPlayCinematicIntroEvent();
			ShowIntro();
			return;
		}
#endif

		if (GameManager.instance.localPlayer.playerSettings.skipIntro)
		{
			sceneFsm.TriggerState(MenuSceneFsm.UninitState);
		}
		else
		{
			RegisterPlayCinematicIntroEvent();
			ShowIntro();
		}
	}

	private void RegisterPlayCinematicIntroEvent()
	{
		EventMessageManager.instance.RemoveHandler(typeof(PlayCinematicCompletedEvent).Name, this);
		EventMessageHandler playCinematicCompletedMessageHandler = new EventMessageHandler(this, OnPlayCinematicCompleted);
		EventMessageManager.instance.AddHandler(typeof(PlayCinematicCompletedEvent).Name, playCinematicCompletedMessageHandler);
	}

	public void ShowIntro()
	{
		sceneFsm.TriggerState(MenuSceneFsm.IntroState);
	}

	public void WatchCinematic()
	{
		EventMessageManager.instance.RemoveHandler(typeof(PlayCinematicCompletedEvent).Name, this);
		EventMessageHandler playCinematicCompletedMessageHandler = new EventMessageHandler(this, OnWatchCinematicCompleted);
		EventMessageManager.instance.AddHandler(typeof(PlayCinematicCompletedEvent).Name, playCinematicCompletedMessageHandler);
		ShowIntro();

	}

	private void OnWatchCinematicCompleted(EventMessage eventMessage)
	{
		PlayCinematicCompletedEvent playCinematicCompletedEvent = eventMessage.eventObject as PlayCinematicCompletedEvent;
		if (playCinematicCompletedEvent.cinematicMenu == introCinematicMenu)
		{
			GameManager.instance.localPlayer.playerSettings.skipIntro = true;
			GameManager.instance.SavePlayer();

			sceneFsm.TriggerState(MenuSceneFsm.MenuState);
		}
	}

	public void ShowCredits()
	{
		sceneFsm.TriggerState(MenuSceneFsm.CreditsState);
	}

	public void WatchCredits()
	{
		EventMessageManager.instance.RemoveHandler(typeof(PlayCreditsCompletedEvent).Name, this);
		EventMessageHandler playCreditsCompletedMessageHandler = new EventMessageHandler(this, OnWatchCreditsCompleted);
		EventMessageManager.instance.AddHandler(typeof(PlayCreditsCompletedEvent).Name, playCreditsCompletedMessageHandler);
		ShowCredits();
	}

	private void OnWatchCreditsCompleted(EventMessage eventMessage)
	{
		sceneFsm.TriggerState(MenuSceneFsm.MenuState);
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
			highScoreGroup.gameObject.SetActive(true);
			highScoreText.DisplayDayHighScore(highScore.day);
		}
		else
		{
			highScoreGroup.gameObject.SetActive(false);
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

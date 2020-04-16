using UnityEngine;
using UnityEngine.SceneManagement;
using Messages;

public class MenuScene : MonoSingleton
{
    public const string NAME = "Menu";
	public GameObject playButton;
	public CinematicMenu introCinematicMenu;
	public GameObject menuCanvas;
	public GameObject cinematicCanvas;

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
			sceneFsm.TriggerState(MenuSceneFsm.MenuState);
		}
	}

	public void OnPlayClick()
    {
		Debug.Log("OnPlayClick()");
		ScenesFlowManager.instance.UnloadingMenuScene(); // PABLO: what if we had 2 navigations?
	}

	public void SetupScene()
	{
		menuCanvas.SetActive(false);
		cinematicCanvas.SetActive(false);
		InitScene.instance.loadingPanel.Exit(OnLoadingPanelExitCompleted);
	}

	private void OnLoadingPanelExitCompleted()
	{
		if (GameManager.instance.localPlayer.playerSettings.skipIntro)
		{
			sceneFsm.TriggerState(MenuSceneFsm.MenuState);
		}
		else
		{
			sceneFsm.TriggerState(MenuSceneFsm.IntroState);
		}
	}

	public void DestroyScene()
	{
		EventMessageManager.instance.RemoveHandler(typeof(PlayCinematicCompletedEvent).Name, this);

		Destroy(gameObject);
		Resources.UnloadUnusedAssets();
	}
}

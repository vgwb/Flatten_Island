using UnityEngine;
using Messages;

public class TutorialGamePhase : IGamePhase
{
	private GamePhaseXmlModel gamePhaseXmlModel;

	private int startDay;

	private AudioSource musicAudioSource;

	private IAdvisorSpawnPolicy advisorSpawnPolicy;
	private GameSession gameSession;

	public void Start(GameSession gameSession, int gamePhaseId, int startDay)
	{
		this.gameSession = gameSession;

		RegisterEventSubscribers();

		advisorSpawnPolicy = new TutorialAdvisorSpawnPolicy();
		advisorSpawnPolicy.Initialize();

		gamePhaseXmlModel = XmlModelManager.instance.FindModel<GamePhaseXmlModel>(gamePhaseId);
		this.startDay = startDay;
	}

	public void Resume(GameSession gameSession)
	{
		this.gameSession = gameSession;

		RegisterEventSubscribers();

		advisorSpawnPolicy = new TutorialAdvisorSpawnPolicy();
		advisorSpawnPolicy.Initialize();
		StartMusic();
	}

	public void StartMusic()
	{
		if (!string.IsNullOrEmpty(gamePhaseXmlModel.musicAudioClip))
		{
			AudioClip musicAudioClip = Resources.Load<AudioClip>(gamePhaseXmlModel.musicAudioClip);
			musicAudioSource = AudioManager.instance.PlayMusic(musicAudioClip);
		}
	}

	public void Stop()
	{
		UnregisterEventSubscribers();

		TutorialMenu.instance.HideAdvisorPortrait();
		StopMusic();
	}

	public void Dispose()
	{
		StopMusic();
	}

	public void StopMusic()
	{
		if (musicAudioSource != null)
		{
			AudioManager.instance.StopMusic(musicAudioSource);
			musicAudioSource = null;
		}
	}

	private void RegisterEventSubscribers()
	{
		EventMessageHandler tutorialDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnTutorialDialogExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(TutorialDialogExitCompletedEvent).Name, tutorialDialogExitCompletedMessageHandler);
	}

	private void UnregisterEventSubscribers()
	{
		EventMessageManager.instance.RemoveHandler(typeof(TutorialDialogExitCompletedEvent).Name, this);
	}

	public void OnTutorialDialogExitCompleted(EventMessage eventMessage)
	{
		TutorialDialogExitCompletedEvent tutorialDialogExitCompletedEvent = eventMessage.eventObject as TutorialDialogExitCompletedEvent;

		if (tutorialDialogExitCompletedEvent.tutorialDialog.advisorXmlModel == null)
		{
			NextDayEntry nextDayEntry = gameSession.ShowNextDayEntry();
			nextDayEntry.PlayEnterRecipe();
		}
	}

	public string GetName()
	{
		return gamePhaseXmlModel.name;
	}

	public int GetNextPhaseId()
	{
		return gamePhaseXmlModel.nextPhaseId;
	}

	public IAdvisorSpawnPolicy GetAdvisorSpawnPolicy()
	{
		return advisorSpawnPolicy;
	}

	public int GetPhaseId()
	{
		return gamePhaseXmlModel.id;
	}

	public int GetStartDay()
	{
		return startDay;
	}

	public void Advisor_Enter()
	{
		if (!gameSession.HasAdvisors())
		{
			gameSession.advisors = AdvisorsManager.instance.PickAdvisors();
		}

		GameManager.instance.SavePlayer();

		TutorialMenu.instance.ShowAdvisorPresentation(gameSession.advisors[0]);
	}

	public void NextDayConfirmation_Enter()
	{
		gameSession.UpdateNextDayValues();

		TutorialMenu.instance.ShowNextDayTipDialog();
	}

	public bool IsFinished()
	{
		if (gamePhaseXmlModel.endConditions == null)
		{
			return false;
		}

		return gamePhaseXmlModel.endConditions.IsSatisfied(gameSession);
	}

	public GameData WriteSaveData()
	{
		GamePhaseData gamePhaseData = new GamePhaseData();
		gamePhaseData.gamePhaseId = gamePhaseXmlModel.id;
		gamePhaseData.startDay = startDay;
		return gamePhaseData;
	}

	public void ReadSaveData(GameData gameData)
	{
		GamePhaseData gamePhaseData = gameData as GamePhaseData;
		gamePhaseXmlModel = XmlModelManager.instance.FindModel<GamePhaseXmlModel>(gamePhaseData.gamePhaseId);
		startDay = gamePhaseData.startDay;
	}
}
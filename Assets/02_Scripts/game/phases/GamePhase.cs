using UnityEngine;

public class GamePhase : IGamePhase
{
	private GamePhaseXmlModel gamePhaseXmlModel;

	private int startDay;

	private AudioSource musicAudioSource;

	private IAdvisorSpawnPolicy advisorSpawnPolicy;

	private GameSession gameSession;

	public void Start(GameSession gameSession, int gamePhaseId, int startDay)
	{
		this.gameSession = gameSession; 

		advisorSpawnPolicy = new AdvisorRandomSpawnPolicy();
		advisorSpawnPolicy.Initialize();

		gamePhaseXmlModel = XmlModelManager.instance.FindModel<GamePhaseXmlModel>(gamePhaseId);
		this.startDay = startDay;
	}

	public void Resume(GameSession gameSession)
	{
		this.gameSession = gameSession;

		advisorSpawnPolicy = new AdvisorRandomSpawnPolicy();
		advisorSpawnPolicy.Initialize();

		StartMusic();
	}

	public void StartMusic()
	{
		if (!string.IsNullOrEmpty(gamePhaseXmlModel.musicAudioClip))
		{
			AudioClip musicAudioClip = Resources.Load<AudioClip>(gamePhaseXmlModel.musicAudioClip);
			musicAudioSource = AudioManager.instance.PlayMusic(musicAudioClip, gamePhaseXmlModel.musicVolume);
		}
	}

	public void Stop()
	{
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

	public void DayStart_Enter()
	{
		Hud.instance.UpdateDayValues();
		GameManager.instance.SavePlayer();
	}

	public void ChangeGamePhase_Enter()
	{
		int nextPhaseId = gameSession.gamePhase.GetNextPhaseId();
		gameSession.gamePhase.Stop();
		gameSession.StartGamePhase(nextPhaseId);
	}

	public void Advisor_Enter()
	{
		if (!gameSession.HasAdvisors())
		{
			gameSession.advisors = AdvisorsManager.instance.PickAdvisors();
		}

		GameManager.instance.SavePlayer();

		AdvisorsManager.instance.ShowAdvisors(gameSession.advisors);
	}

	public void UpdateResult_Enter()
	{
		Hud.instance.UpdateSuggestionOptions();
		ChartManager.instance.RestartCurrentDayChartAnimation();
		Hud.instance.UpdateDayValues();
	}

	public void UpdateResult_Update(GameSessionFsm gameSessionFsm)
	{
		gameSessionFsm.TriggerState(GameSessionFsm.NextDayConfirmationState);
	}

	public void NextDayConfirmation_Enter()
	{
		gameSession.UpdateNextDayValues();

		NextDayEntry nextDayEntry = gameSession.ShowNextDayEntry();
		nextDayEntry.PlayEnterRecipe();
	}

	public void NextDayConfirmation_Exit()
	{
		ChartManager.instance.RestartChartAnimation();
		gameSession.NextDay();
	}

	public string GetName()
	{
		return gamePhaseXmlModel.name;
	}

	public int GetNextPhaseId()
	{
		return gamePhaseXmlModel.nextPhaseId;
	}

	public int GetPhaseId()
	{
		return gamePhaseXmlModel.id;
	}

	public int GetStartDay()
	{
		return startDay;
	}

	public IAdvisorSpawnPolicy GetAdvisorSpawnPolicy()
	{
		return advisorSpawnPolicy;
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
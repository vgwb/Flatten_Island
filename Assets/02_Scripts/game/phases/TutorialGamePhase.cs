﻿using UnityEngine;

public class TutorialGamePhase : IGamePhase
{
	private GamePhaseXmlModel gamePhaseXmlModel;

	private int startDay;

	private AudioSource musicAudioSource;

	private IAdvisorSpawnPolicy advisorSpawnPolicy;

	public void Start(int gamePhaseId, int startDay)
	{
		advisorSpawnPolicy = new TutorialAdvisorSpawnPolicy();
		advisorSpawnPolicy.Initialize();

		gamePhaseXmlModel = XmlModelManager.instance.FindModel<GamePhaseXmlModel>(gamePhaseId);
		this.startDay = startDay;
	}

	public void Resume()
	{
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
		AdvisorsManager.instance.HideAdvisorTutorialPortrait();
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

	public void Advisor_Enter(GameSession gameSession)
	{
		if (!gameSession.HasAdvisors())
		{
			gameSession.advisors = AdvisorsManager.instance.PickAdvisors();
		}

		GameManager.instance.SavePlayer();

		AdvisorsManager.instance.ShowAdvisorPresentation(gameSession.advisors[0]);
	}

	public bool IsFinished(GameSession gameSession)
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
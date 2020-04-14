using UnityEngine;
using Messages;

public class GamePhase : ISavable
{
	private GamePhaseXmlModel gamePhaseXmlModel;

	private int startDay;

	private AudioSource musicAudioSource;

	public void Start(int gamePhaseId, int startDay)
	{
		gamePhaseXmlModel = XmlModelManager.instance.FindModel<GamePhaseXmlModel>(gamePhaseId);
		this.startDay = startDay;

		StartMusic();
	}

	public void Resume()
	{
		StartMusic();
	}

	private void StartMusic()
	{
		if (!string.IsNullOrEmpty(gamePhaseXmlModel.musicAudioClip))
		{
			AudioClip musicAudioClip = Resources.Load<AudioClip>(gamePhaseXmlModel.musicAudioClip);
			musicAudioSource = AudioManager.instance.PlayMusic(musicAudioClip);
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

	private void StopMusic()
	{
		if (musicAudioSource != null)
		{
			AudioManager.instance.StopMusic(musicAudioSource);
			musicAudioSource = null;
		}
	}

	public string GetName()
	{
		return LocalizationManager.instance.GetText(gamePhaseXmlModel.name);
	}

	public int GetNextPhaseId()
	{
		return gamePhaseXmlModel.nextPhaseId;
	}

	public int GetStartDay()
	{
		return startDay;
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
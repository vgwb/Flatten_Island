using UnityEngine;
using System.Collections;

public class LocalPlayer : Player
{
	public GameSession gameSession;
	public PlayerSettings playerSettings;

	public LocalPlayer()
	{
		Init();
	}

	public void Init()
	{
		gameSession = null;
		playerSettings = new PlayerSettings();
	}

	public bool HasSession()
	{
		//The check day > 0 is needed because Unity Json Serializer serialize a null object with its default values :-(
		return gameSession != null && gameSession.day > 0;
	}

	public void StartNewGameSession()
	{
		gameSession = new GameSession();
		gameSession.Start();
	}

	public void ResumeGameSession()
	{
		gameSession.Resume();
	}

	public void QuitGameSession()
	{
		gameSession.Dispose();
		gameSession = null;
	}

	public override GameData WriteSaveData()
	{
		LocalPlayerData localPlayerData = new LocalPlayerData();

		PlayerSettingsData playerSettingsData = playerSettings.WriteSaveData() as PlayerSettingsData;
		localPlayerData.playerSettingsData = playerSettingsData;

		if (gameSession != null)
		{
			GameSessionData gameSessionData = gameSession.WriteSaveData() as GameSessionData;
			localPlayerData.gameSessionData = gameSessionData;
		}
		else
		{
			localPlayerData.gameSessionData = null;
		}

		return localPlayerData;
	}

	public override void ReadSaveData(GameData gameData)
	{
		LocalPlayerData localPlayerData = gameData as LocalPlayerData;

		if (localPlayerData.gameSessionData != null)
		{
			gameSession = new GameSession();
			gameSession.ReadSaveData(localPlayerData.gameSessionData);
		}
		else
		{
			gameSession = null;
		}

		if (localPlayerData.playerSettingsData != null)
		{
			playerSettings.ReadSaveData(localPlayerData.playerSettingsData);
		}
		else
		{
			playerSettings = new PlayerSettings();
		}
	}
}

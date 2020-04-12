using UnityEngine;
using System.Collections;

public class LocalPlayer : Player
{
	public bool skipIntro;

	public GameSession gameSession;

	public LocalPlayer()
	{
		gameSession = null;
	}

	public void Init()
	{
		gameSession = null;
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

	public void QuitGameSession()
	{
		gameSession = null;
	}

	public override GameData WriteSaveData()
	{
		LocalPlayerData localPlayerData = new LocalPlayerData();
		localPlayerData.skipIntro = skipIntro;

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

		skipIntro = localPlayerData.skipIntro;
	}
}

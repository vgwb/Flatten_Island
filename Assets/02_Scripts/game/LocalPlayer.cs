using System.Collections.Generic;

public class LocalPlayer : Player
{
	public GameSession gameSession;
	public PlayerSettings playerSettings;
	public HighScore highScore;
	public Statistics statistics;

	public LocalPlayer()
	{
		Init();
	}

	public void Init()
	{
		gameSession = null;
		playerSettings = new PlayerSettings();
		highScore = new HighScore();
		statistics = new Statistics();
	}

	public bool HasSession()
	{
		//The capacity > 0 is needed because Unity Json Serializer serialize a null object with its default values :-(
		return gameSession != null && gameSession.capacity > 0;
	}

	public bool HasLanguageId()
	{
		return playerSettings.HasLanguageId();
	}

	public void SetLanguageId(string languageId)
	{
		playerSettings.SetLanguageId(languageId);
	}

	public string GetLanguageId()
	{
		return playerSettings.GetLanguageId();
	}

	public void StartNewGameSession()
	{
		gameSession = new GameSession();

		int gamePhaseId = GetInitialGamePhaseId();

		gameSession.Initialize(this);
		gameSession.Start(gamePhaseId);

		statistics.IncreaseTotGamesPlayed();
	}

	private int GetInitialGamePhaseId()
	{
		if (playerSettings.showTutorial)
		{
			return GameSession.TUTORIAL_GAME_PHASE_ID;
		}
		else
		{
			return GameSession.INITIAL_PHASE_ID;
		}
	}

	public void ResumeGameSession()
	{
		gameSession.Resume();
	}

	public void QuitGameSession()
	{
		statistics.IncreaseGamesQuit();

		gameSession.Dispose();
		gameSession = null;
	}

	public bool TryUpdateDayHighScore()
	{
		return highScore.TryUpdateDayHighScore(gameSession.day);
	}

	public bool TryUpdateGrowthRateHighScore()
	{
		return highScore.TryUpdateGrowthRateHighScore(gameSession.growthRate);
	}

	public bool TryUpdatePublicOpinionHighScore()
	{
		return highScore.TryUpdatePublicOpinionHighScore(gameSession.publicOpinion);
	}

	public void UpdateStatistics()
	{
		if (gameSession.HasPlayerLose())
		{
			if (gameSession.HasPlayerLoseDueCapacity())
			{
				statistics.IncreaseGamesLostForPatients();
			}
			else if (gameSession.HasPlayerLoseDueMoney())
			{
				statistics.IncreaseGamesLostForMoney();
			}
			else if (gameSession.HasPlayerLoseDuePublicOpinion())
			{
				statistics.IncreaseGamesLostForPublicOpinion();
			}
		}
		else if (gameSession.HasPlayerWon())
		{
			statistics.IncreaseGamesWon();
		}
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

		HighScoreData highScoreData = highScore.WriteSaveData() as HighScoreData;
		localPlayerData.highScoreData = highScoreData;

		StatisticsData statisticsData = statistics.WriteSaveData() as StatisticsData;
		localPlayerData.statisticsData = statisticsData;

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

		if (localPlayerData.highScoreData != null)
		{
			highScore.ReadSaveData(localPlayerData.highScoreData);
		}
		else
		{
			highScore = new HighScore();
		}

		if (localPlayerData.statisticsData != null)
		{
			statistics.ReadSaveData(localPlayerData.statisticsData);
		}
		else
		{
			statistics = new Statistics();
		}
	}
}

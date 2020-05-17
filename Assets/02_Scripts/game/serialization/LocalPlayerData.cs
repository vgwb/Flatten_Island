﻿using System;

[Serializable]
public class LocalPlayerData : GameData
{
	public GameSessionData gameSessionData;
	public PlayerSettingsData playerSettingsData;
	public HighScoreData highScoreData;
	public StatisticsData statisticsData;
}
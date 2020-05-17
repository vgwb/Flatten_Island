public class Statistics : ISavable
{
	public int totGamesPlayed { get; private set; }
	public int gamesWon { get; private set; }
	public int gamesQuit { get; private set; }
	public int gamesLostForMoney { get; private set; }
	public int gamesLostForPublicOpinion { get; private set; }
	public int gamesLostForPatients { get; private set; }

	public Statistics()
	{
		totGamesPlayed = 0;
		gamesWon = 0;
		gamesQuit = 0;
		gamesLostForMoney = 0;
		gamesLostForPublicOpinion = 0;
		gamesLostForPatients = 0;
	}

	public void IncreaseTotGamesPlayed()
	{
		totGamesPlayed++;
	}

	public void IncreaseGamesWon()
	{
		gamesWon++;
	}

	public void IncreaseGamesQuit()
	{
		gamesQuit++;
	}

	public void IncreaseGamesLostForMoney()
	{
		gamesLostForMoney++;
	}

	public void IncreaseGamesLostForPublicOpinion()
	{
		gamesLostForPublicOpinion++;
	}

	public void IncreaseGamesLostForPatients()
	{
		gamesLostForPatients++;
	}

	public GameData WriteSaveData()
	{
		StatisticsData statisticsData = new StatisticsData();
		statisticsData.totGamesPlayed = totGamesPlayed;
		statisticsData.gamesWon = gamesWon;
		statisticsData.gamesQuit = gamesQuit;
		statisticsData.gamesLostForMoney = gamesLostForMoney;
		statisticsData.gamesLostForPublicOpinion = gamesLostForPublicOpinion;
		statisticsData.gamesLostForPatients = gamesLostForPatients;

		return statisticsData;
	}

	public void ReadSaveData(GameData gameData)
	{
		StatisticsData statistics = gameData as StatisticsData;
		totGamesPlayed = statistics.totGamesPlayed;
		gamesWon = statistics.gamesWon;
		gamesQuit = statistics.gamesQuit;
		gamesLostForMoney = statistics.gamesLostForMoney;
		gamesLostForPublicOpinion = statistics.gamesLostForPublicOpinion;
		gamesLostForPatients = statistics.gamesLostForPatients;
	}
}
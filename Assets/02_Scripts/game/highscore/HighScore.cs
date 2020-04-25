public class HighScore : ISavable
{
	public int day { get; private set; }
	public int growthRate { get; private set; }
	public int publicOpinion { get; private set; }

	public HighScore()
	{
		day = int.MaxValue;
		growthRate = int.MaxValue;
		publicOpinion = int.MinValue;
	}

	public bool HasDayHighScore()
	{
		return day < int.MaxValue;
	}

	public bool TryUpdateDayHighScore(int day)
	{
		if (this.day > day)
		{
			this.day = day;
			return true;
		}

		return false;
	}

	public bool TryUpdateGrowthRateHighScore(int growthRate)
	{
		if (this.growthRate > growthRate)
		{
			this.growthRate = growthRate;
			return true;
		}

		return false;
	}

	public bool TryUpdatePublicOpinionHighScore(int publicOpinion)
	{
		if (this.publicOpinion < publicOpinion)
		{
			this.publicOpinion = publicOpinion;
			return true;
		}

		return false;
	}

	public GameData WriteSaveData()
	{
		HighScoreData highScore = new HighScoreData();
		highScore.day = day;
		highScore.growthRate = growthRate;
		highScore.publicOpinion = publicOpinion;

		return highScore;
	}

	public void ReadSaveData(GameData gameData)
	{
		HighScoreData highScoreData = gameData as HighScoreData;
		day = highScoreData.day;
		growthRate = highScoreData.growthRate;
		publicOpinion = highScoreData.publicOpinion;
	}
}
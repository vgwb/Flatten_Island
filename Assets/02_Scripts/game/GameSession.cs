public class GameSession : ISavable
{
	public int day { get; set; }
	public int[] patients { get; set; }
	public int vaccineDevelopment { get; set; }
	public int growthRate { get; set; }
	public int capacity { get; set; }
	public int money { get; set; }
	public float publicOpinion { get; set; }

	public GameSession()
	{
		//temp
		patients = new int[LocalPlayer.MAX_DAYS];
		day = 1;
		vaccineDevelopment = 0;
		patients[0] = 1000;
		growthRate = 5;
		capacity = 1000;
		money = 10000;
		publicOpinion = 0.5f;
	}

	public GameData WriteSaveData()
	{
		GameSessionData gameSessionData = new GameSessionData();
		gameSessionData.day = day;
		gameSessionData.growthRate = growthRate;
		gameSessionData.money = money;
		gameSessionData.vaccineDevelopment = vaccineDevelopment;
		gameSessionData.capacity = capacity;
		gameSessionData.publicOpinion = publicOpinion;
		gameSessionData.patients = (int[]) patients.Clone();
		return gameSessionData;
	}

	public void ReadSaveData(GameData gameData)
	{
		GameSessionData gameSessionData = gameData as GameSessionData;
		day = gameSessionData.day;
		growthRate = gameSessionData.growthRate;
		money = gameSessionData.money;
		vaccineDevelopment = gameSessionData.vaccineDevelopment;
		capacity = gameSessionData.capacity;
		publicOpinion = gameSessionData.publicOpinion;
		patients = (int[])gameSessionData.patients.Clone();
	}
}
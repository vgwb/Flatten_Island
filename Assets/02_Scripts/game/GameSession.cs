using System.Collections.Generic;

public class GameSession : ISavable
{
	public int day { get; set; }
	public int[] patients { get; set; }
	public int vaccineDevelopment { get; set; }
	public int growthRate { get; set; }
	public int capacity { get; set; }
	public int money { get; set; }
	public float publicOpinion { get; set; }
	public List<AdvisorXmlModel> advisors;

	public void Start()
	{
		//temp
		patients = new int[LocalPlayer.MAX_DAYS];
		advisors = AdvisorsManager.instance.PickAdvisors();
		day = 1;
		vaccineDevelopment = 0;
		patients[0] = 1000;
		growthRate = 5;
		capacity = 1000;
		money = 10000;
		publicOpinion = 0.5f;
	}

	public void NextDay()
	{
		//[TO] other parameter to modify here 
		advisors = AdvisorsManager.instance.PickAdvisors();
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

		gameSessionData.advisorIds = new int[advisors.Count];
		for (int i = 0; i < advisors.Count; i++)
		{
			gameSessionData.advisorIds[i] = advisors[i].id;
		}

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
		patients = (int[]) gameSessionData.patients.Clone();

		advisors = new List<AdvisorXmlModel>();
		for (int i = 0; i < gameSessionData.advisorIds.Length; i++)
		{
			AdvisorXmlModel advisorXmlModel = XmlModelManager.instance.FindModel<AdvisorXmlModel>(gameSessionData.advisorIds[i]);
			if (advisorXmlModel != null)
			{
				advisors.Add(advisorXmlModel);
			}
		}
	}
}
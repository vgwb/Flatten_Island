using System.Collections.Generic;

public class GameSession : ISavable
{
	public const int MAX_DAYS = 110;
	public const int MAX_PATIENTS = 10000;

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
		patients = new int[MAX_DAYS];
		advisors = AdvisorsManager.instance.PickAdvisors();
		day = 1;
		vaccineDevelopment = 0;
		patients[0] = 1000;
		growthRate = 5;
		capacity = 1000;
		money = 10000;
		publicOpinion = 0.5f;
	}

	public void ApplySuggestionOption(SuggestionOptionXmlModel selectedSuggestionOptionXmlModel)
	{
		money += selectedSuggestionOptionXmlModel.moneyModifier;
		growthRate += selectedSuggestionOptionXmlModel.growthRateModifier;
		publicOpinion += selectedSuggestionOptionXmlModel.publicOpinionModifier;
		capacity += selectedSuggestionOptionXmlModel.capacityModifier;
		int patientsIncrease = (patients[day - 1] * growthRate) / 100;
		patients[day] = patients[day - 1] + patientsIncrease;
	}

	public void NextDay()
	{
		money += patients[day] * 3 - capacity * 2;
		vaccineDevelopment++;
		day++;

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

		// TODO Review. seems like the previous code can fail, but this might not be good either
		// Probably if any of these data (patients or advisorIds) fail to load we want to reset
		if (gameSessionData.patients != null)
		{
			patients = (int[]) gameSessionData.patients.Clone(); 
		}
		else
		{
			patients = new int[MAX_DAYS];
		}

		advisors = new List<AdvisorXmlModel>();
		if (gameSessionData.advisorIds != null)
		{
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
}
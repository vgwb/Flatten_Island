using System.Collections.Generic;

public class GameSession : ISavable
{
	public const int MAX_DAYS = 110; // TODO
	public const int MAX_PATIENTS = 30000; // TODO

	public int day { get; set; }
	public int[] patients { get; set; }
	public int vaccineDevelopment { get; set; }
	public int growthRate { get; set; }
	public int capacity { get; set; }
	public int money { get; set; }
	public int publicOpinion { get; set; }
	public List<AdvisorXmlModel> advisors;

	private GameSessionXmlModel gameSessionXmlModel;

	public void Start()
	{
		if (gameSessionXmlModel == null)
		{
			gameSessionXmlModel = XmlModelManager.instance.FindModel<GameSessionXmlModel>();
		}


		patients = new int[MAX_DAYS];
		advisors = AdvisorsManager.instance.PickAdvisors();
		day = 1;
		vaccineDevelopment = gameSessionXmlModel.initialVaccineDevelopment;
		patients[0] = gameSessionXmlModel.initialPatients;
		growthRate = gameSessionXmlModel.initialGrowthRate;
		capacity = gameSessionXmlModel.initialCapacity;
		money = gameSessionXmlModel.initialMoney;
		publicOpinion = gameSessionXmlModel.initialPublicOpinion;
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
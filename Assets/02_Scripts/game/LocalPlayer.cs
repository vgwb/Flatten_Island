using UnityEngine;
using System.Collections;

public class LocalPlayer : ISavable
{
	public const int MAX_DAYS = 110;
	public const int MAX_PATIENTS = 10000;

	public bool skipIntro;

	public int day { get; set; }
	public int[] patients { get; set; }
	public int vaccineDevelopment { get; set; }
	public int growthRate { get; set; }
	public int capacity { get; set; }
	public int money { get; set; }
	public float publicOpinion { get; set; }
	public Suggestion[] suggestions { get; private set; }
	public Suggestion adviced { get; private set; }

	private GameSession gameSession;

	public LocalPlayer()
	{
		patients = new int[MAX_DAYS];
		gameSession = null;
	}

	public void Init()
	{
		// TODO provisional initial state
		day = 1;
		vaccineDevelopment=0;
		patients[0] = 1000;
		growthRate = 5;
		capacity = 1000;
		money = 10000;
		publicOpinion = 0.5f;

		gameSession = new GameSession();
	}

	public void IncreaseDay()
	{
		money += patients[day] * 3 - capacity * 2;
		vaccineDevelopment++;
		day++;
	}

	public void ApplySuggestionOption(SuggestionOptionXmlModel selectedSuggestionOptionXmlModel)
	{
		money += selectedSuggestionOptionXmlModel.moneyModifier;
		growthRate += selectedSuggestionOptionXmlModel.growthRateModifier;
		publicOpinion += selectedSuggestionOptionXmlModel.publicOpinionModifier;
		capacity += selectedSuggestionOptionXmlModel.capacityModifier;
		int patientsIncrease = (patients[day - 1] * growthRate) / 100;
		patients[day] = patients[day - 1] + patientsIncrease;
		IncreaseDay();
	}

	public GameData WriteSaveData()
	{
		LocalPlayerData localPlayerData = new LocalPlayerData();
		localPlayerData.skipIntro = skipIntro;

		if (gameSession != null)
		{
			//Begin TEMP
			gameSession.day = day;
			gameSession.patients = (int[]) patients.Clone();
			gameSession.money = money;
			gameSession.publicOpinion = publicOpinion;
			gameSession.growthRate = growthRate;
			gameSession.capacity = capacity;
			//END TEMP

			GameSessionData gameSessionData = gameSession.WriteSaveData() as GameSessionData;
			localPlayerData.gameSessionData = gameSessionData;
		}

		return localPlayerData;
	}

	public void ReadSaveData(GameData gameData)
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

using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSession : ISavable
{
	public static int INITIAL_PHASE_ID = 2;

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
	public GamePhase gamePhase;

	private GameSessionXmlModel gameSessionXmlModel;
	private GameSessionFsm gameSessionFsm;

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

		StartGamePhase(INITIAL_PHASE_ID);

		StartFsm();
	}

	private void StartFsm()
	{
		gameSessionFsm = new GameSessionFsm(this);
		gameSessionFsm.StartFsm();
	}

	public void Dispose()
	{
		gamePhase.Dispose();

		if (gameSessionFsm != null)
		{
			gameSessionFsm.Dispose();
			gameSessionFsm = null;
		}
	}

	public void Resume()
	{
		StartFsm();
		gamePhase.Resume();
	}

	public void UpdateFsm()
	{
		gameSessionFsm.Update();
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
	}

	public void PickAdvisors()
	{
		advisors = AdvisorsManager.instance.PickAdvisors();
	}

	public bool IsCurrentPhaseFinished()
	{
		return gamePhase.IsFinished(this);
	}

	public bool HasPlayerWon()
	{
		//TODO
		return false;
	}

	public bool HasPlayerLose()
	{
		//TODO
		return false;
	}


	public void StartGamePhase(int gamePhaseId)
	{
		gamePhase = new GamePhase();
		gamePhase.Start(gamePhaseId, day);

		Debug.Log("GameSession = Starting Game Phase:" + gamePhase.GetName());

		//To Do: Show Game Phase Start Dialog or Intro Sequence
	}

	public NextDayEntry ShowNextDayEntry()
	{
		Transform parentTransform = MainScene.instance.suggestionMenu.transform;
		GameObject nextDayEntry = GameObjectFactory.instance.InstantiateGameObject(NextDayEntry.PREFAB, parentTransform, false);
		nextDayEntry.gameObject.transform.SetParent(parentTransform, true);
		NextDayEntry nextDayEntryScript = nextDayEntry.GetComponent<NextDayEntry>();
		nextDayEntry.gameObject.SetActive(true);
		return nextDayEntryScript;
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

		GamePhaseData gamePhaseData = gamePhase.WriteSaveData() as GamePhaseData;
		gameSessionData.gamePhaseData = gamePhaseData;

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

		gamePhase = new GamePhase();
		gamePhase.ReadSaveData(gameSessionData.gamePhaseData);
	}
}
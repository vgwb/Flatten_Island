using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameSession : ISavable
{
	public static int TUTORIAL_GAME_PHASE_ID = 0;
	public static int INITIAL_PHASE_ID = 1;
	private const int MIN_GROWTH_RATE = -50;

	public const int MAX_DAYS = 110; // TODO
	public const int MAX_PATIENTS = 30000; // TODO

	private const int VACCINE_PROGRESS_END_DEVELOPMENT = 100;

	public int day;
	public int[] patients;
	public int vaccineDevelopment;
	public int growthRate;
	public int capacity;
	public int money;
	public int publicOpinion;
	public List<AdvisorXmlModel> advisors;
	public IGamePhase gamePhase;
	public int gamePhaseId;
	public List<GameStoryXmlModel> activeGameStories;

	private GameSessionXmlModel gameSessionXmlModel;
	private GameSessionFsm gameSessionFsm;

	private ISuggestionSelectionPolicy suggestionSelectionPolicy;

	public GameSession()
	{
		if (gameSessionXmlModel == null)
		{
			gameSessionXmlModel = XmlModelManager.instance.FindModel<GameSessionXmlModel>();
		}
	}

	public void Initialize()
	{
		patients = new int[MAX_DAYS];
		activeGameStories = new List<GameStoryXmlModel>();
		day = 1;
		advisors = new List<AdvisorXmlModel>();
		vaccineDevelopment = gameSessionXmlModel.initialVaccineDevelopment;
		patients[0] = gameSessionXmlModel.initialPatients;
		growthRate = gameSessionXmlModel.initialGrowthRate;
		capacity = gameSessionXmlModel.initialCapacity;
		money = gameSessionXmlModel.initialMoney;
		publicOpinion = gameSessionXmlModel.initialPublicOpinion;
	}

	public void Start(int initialGamePhaseId)
	{
		gamePhaseId = initialGamePhaseId;
		StartGamePhase(initialGamePhaseId);
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

	public bool HasAdvisors()
	{
		return advisors.Count > 0;
	}

	public void DiscardAdvisors()
	{
		advisors.Clear();
	}

	public void ApplySuggestionOption(SuggestionOptionXmlModel selectedSuggestionOptionXmlModel)
	{
		money += selectedSuggestionOptionXmlModel.moneyModifier;
		growthRate = Math.Max(MIN_GROWTH_RATE, growthRate + selectedSuggestionOptionXmlModel.growthRateModifier);
		publicOpinion = Math.Min(100, publicOpinion + selectedSuggestionOptionXmlModel.publicOpinionModifier);
		publicOpinion = Math.Max(0, publicOpinion);
		capacity = Math.Max(0, capacity + selectedSuggestionOptionXmlModel.capacityModifier);
		IncrementVaccineDevelopment(selectedSuggestionOptionXmlModel.vaccineModifier);

		TryStartStory(selectedSuggestionOptionXmlModel);

		TryStopStory(selectedSuggestionOptionXmlModel);
	}

	private void IncrementVaccineDevelopment(int increment)
	{
		vaccineDevelopment = Math.Min(VACCINE_PROGRESS_END_DEVELOPMENT, vaccineDevelopment + increment);
		vaccineDevelopment = Math.Max(0, vaccineDevelopment);
	}

	private void TryStartStory(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		if (suggestionOptionXmlModel.HasStartStoryId())
		{
			GameStoryXmlModel gameStoryXmlModel = XmlModelManager.instance.FindModel<GameStoryXmlModel>(suggestionOptionXmlModel.GetStartStoryId());
			if (gameStoryXmlModel != null && !activeGameStories.Contains(gameStoryXmlModel))
			{
				activeGameStories.Add(gameStoryXmlModel);
			}
		}
	}

	private void TryStopStory(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		if (suggestionOptionXmlModel.HasStopStoryId())
		{
			GameStoryXmlModel gameStoryXmlModel = XmlModelManager.instance.FindModel<GameStoryXmlModel>(suggestionOptionXmlModel.GetStopStoryId());
			if (gameStoryXmlModel != null && activeGameStories.Contains(gameStoryXmlModel))
			{
				activeGameStories.Remove(gameStoryXmlModel);
			}
		}
	}

	public void UpdateNextDayValues()
	{
		money += gameSessionXmlModel.nextDayMoneyIncrement;
		IncrementVaccineDevelopment(gameSessionXmlModel.nextDayVaccineIncrement);
		growthRate += gameSessionXmlModel.nextDayGrowthRateIncrement;
		int patientsIncrease = (patients[day - 1] * growthRate) / 100;
		patients[day] = patients[day - 1] + patientsIncrease;
	}

	public void NextDay()
	{
		day++;
	}

	public bool IsCurrentPhaseFinished()
	{
		return gamePhase.IsFinished(this);
	}

	public bool HasPlayerWon()
	{
		return vaccineDevelopment >= VACCINE_PROGRESS_END_DEVELOPMENT;
	}

	public bool HasPlayerLose()
	{
		if (patients[day - 1] > capacity)
		{
			return true;
		}

		if (capacity <= 0)
		{
			return true;
		}

		if (money <= 0)
		{
			return true;
		}

		if (publicOpinion <= 0)
		{
			return true;
		}

		return false;
	}


	public void StartGamePhase(int nextGamePhaseId)
	{
		if (nextGamePhaseId == TUTORIAL_GAME_PHASE_ID)
		{
			gamePhase = new TutorialGamePhase();
		}
		else
		{
			gamePhase = new GamePhase();
		}

		gamePhaseId = nextGamePhaseId;

		gamePhase.Start(gamePhaseId, day);
		gamePhase.StartMusic();

		Debug.Log("GameSession = Starting Game Phase:" + gamePhase.GetName());
	}

	public NextDayEntry ShowNextDayEntry()
	{
		Transform parentTransform = MainScene.instance.suggestionMenu.transform;
		GameObject nextDayEntry = GameObjectFactory.instance.InstantiateGameObject(NextDayEntry.PREFAB, parentTransform, false);
		nextDayEntry.gameObject.transform.SetParent(parentTransform, true);
		NextDayEntry nextDayEntryScript = nextDayEntry.GetComponent<NextDayEntry>();
		nextDayEntryScript.SetParameters(gameSessionXmlModel, day);
		nextDayEntry.gameObject.SetActive(true);
		nextDayEntry.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return nextDayEntryScript;
	}


	public SuggestionXmlModel PickNextAvailableSuggestion(AdvisorXmlModel advisorXmlModel, LocalPlayer localPlayer)
	{
		if (suggestionSelectionPolicy == null)
		{
			suggestionSelectionPolicy = new SuggestionSelectionRandomPolicy();
			suggestionSelectionPolicy.Initialize(localPlayer);
			suggestionSelectionPolicy.Reset();
		}

		return suggestionSelectionPolicy.GetSuggestion(advisorXmlModel);
	}

	public GameOverEntry ShowWonDialog()
	{
		return ShowGameOverEntry(true);
	}

	public GameOverEntry ShowLoseDialog()
	{
		return ShowGameOverEntry(false);
	}

	private GameOverEntry ShowGameOverEntry(bool hasPlayerWon)
	{
		Transform parentTransform = MainScene.instance.suggestionMenu.transform;
		GameObject gameOverEntry = GameObjectFactory.instance.InstantiateGameObject(GameOverEntry.PREFAB, parentTransform, false);
		gameOverEntry.gameObject.transform.SetParent(parentTransform, true);
		GameOverEntry gameOverEntryScript = gameOverEntry.GetComponent<GameOverEntry>();
		gameOverEntryScript.SetParameters(hasPlayerWon);
		gameOverEntryScript.RefreshCanvas();
		return gameOverEntryScript;
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

		gameSessionData.gamePhaseId = gamePhaseId;
		GamePhaseData gamePhaseData = gamePhase.WriteSaveData() as GamePhaseData;
		gameSessionData.gamePhaseData = gamePhaseData;

		gameSessionData.activeGameStoryIds = new int[activeGameStories.Count];
		for (int i = 0; i < activeGameStories.Count; i++)
		{
			gameSessionData.activeGameStoryIds[i] = activeGameStories[i].id;
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

		gamePhaseId = gameSessionData.gamePhaseId;
		if (gamePhaseId == TUTORIAL_GAME_PHASE_ID)
		{
			gamePhase = new TutorialGamePhase();
		}
		else
		{
			gamePhase = new GamePhase();
		}

		gamePhase.ReadSaveData(gameSessionData.gamePhaseData);

		activeGameStories = new List<GameStoryXmlModel>();

		if (gameSessionData.activeGameStoryIds != null)
		{
			for (int i = 0; i < gameSessionData.activeGameStoryIds.Length; i++)
			{
				GameStoryXmlModel gameStoryXmlModel = XmlModelManager.instance.FindModel<GameStoryXmlModel>(gameSessionData.activeGameStoryIds[i]);
				if (gameStoryXmlModel != null)
				{
					activeGameStories.Add(gameStoryXmlModel);
				}
			}
		}
	}
}
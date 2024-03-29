﻿using System.Collections.Generic;
using System.Text;
using System.IO;

public class GameSessionSimulator
{
	public static string SIMULATION_FILE_PATH = "Assets/10_Sandbox/simulations/SimulationResults.csv";

	public int winRuns { get; private set; }
	public int loseRuns { get; private set; }
	public int winDaysRecord { get; private set; }
	public float epochWinDays { get; private set; }
	public float epochLoseDays { get; private set; }
	public int capacityLoseCount { get; private set; }
	public int publicOpinionLoseCount { get; private set; }
	public int moneyLoseCount { get; private set; }

	private LocalPlayer localPlayer;

	private IAdvisorSpawnPolicy advisorSpawnPolicy;
	private AdvisorXmlModel selectedAdvisorXmlModel;
	private AdvisorXmlModel previousSelectedAdvisorXmlModel;

	private ShuffleBag<IGameSimulatorStrategy> strategiesShuffleBag;
	private ShuffleBag<IGameSimulatorOptionSelectionStrategy> optionSelectionStrategyShuffleBag;

	private GameSessionSimulatorSettings simulatorSettings;

	public void Initialize(GameSessionSimulatorSettings simulatorSettings)
	{
		this.simulatorSettings = simulatorSettings;
		winRuns = 0;
		loseRuns = 0;
		winDaysRecord = int.MaxValue;
		capacityLoseCount = 0;
		publicOpinionLoseCount = 0;
		moneyLoseCount = 0;
		epochWinDays = 0;
		epochLoseDays = 0;

		strategiesShuffleBag = new ShuffleBag<IGameSimulatorStrategy>(100);
		optionSelectionStrategyShuffleBag = new ShuffleBag<IGameSimulatorOptionSelectionStrategy>(100);

		advisorSpawnPolicy = new AdvisorRandomSpawnPolicy();
		advisorSpawnPolicy.Initialize();
		selectedAdvisorXmlModel = null;
		previousSelectedAdvisorXmlModel = null;
	}

	private void InitializeStrategiesShuffleBag()
	{
		strategiesShuffleBag.Clear();
		foreach (GameSimulatorStrategyData strategyData in simulatorSettings.strategiesData)
		{
			int probability = 0;
			if (int.TryParse(strategyData.probabilityText, out probability))
			{
				strategiesShuffleBag.Add(strategyData.strategy, probability);
			}
			else
			{
				UnityEngine.Debug.LogWarning("Probability for strategy:" + strategyData.strategy.ToString() + " is not a number! Skipping");
			}
		}

		strategiesShuffleBag.Shuffle();
	}

	private void InitializeOptionSelectionStrategiesShuffleBag()
	{
		optionSelectionStrategyShuffleBag.Clear();
		foreach (GameSimulatorOptionSelectionStrategyData optionStrategyData in simulatorSettings.optionStrategiesData)
		{
			int probability = 0;
			if (int.TryParse(optionStrategyData.probabilityText, out probability))
			{
				optionSelectionStrategyShuffleBag.Add(optionStrategyData.strategy, probability);
			}
			else
			{
				UnityEngine.Debug.LogWarning("Probability for Option strategy:" + optionStrategyData.strategy.ToString() + " is not a number! Skipping");
			}
		}

		optionSelectionStrategyShuffleBag.Shuffle();
	}

	public void Run()
	{
		StreamWriter writer = new StreamWriter(SIMULATION_FILE_PATH, false);

		for (int i = 0; i < simulatorSettings.simulationRuns; i++)
		{
			InitializePlayer();

			bool gameOver = false;

			GameSimulationResult gameSimulationResult = new GameSimulationResult();
			gameSimulationResult.run = i + 1;

			InitializeStrategiesShuffleBag();
			InitializeOptionSelectionStrategiesShuffleBag();

			while (!gameOver)
			{
				GameSession gameSession = localPlayer.gameSession;
				IGameSimulatorStrategy currentStrategy = strategiesShuffleBag.Next();
				currentStrategy.Initialize(gameSession, simulatorSettings);

				IGameSimulatorOptionSelectionStrategy currentOptionSelectionStrategy = optionSelectionStrategyShuffleBag.Next();
				currentOptionSelectionStrategy.Initialize(gameSession, simulatorSettings);

				GameSimulationResultRow gameSimulationResultRow = new GameSimulationResultRow();
				gameSimulationResultRow.day = gameSession.day;
				gameSimulationResultRow.phase = gameSession.gamePhase.GetPhaseId();
				gameSimulationResultRow.capacity = gameSession.capacity;
				gameSimulationResultRow.growthRate = gameSession.growthRate;

				if (gameSession.day == 0)
				{
					gameSimulationResultRow.patients = gameSession.GetPreviousDayPatientsForTutorialDayZero();
				}
				else
				{
					gameSimulationResultRow.patients = gameSession.patients[gameSession.day - 1];
				}

				gameSimulationResultRow.money = gameSession.money;
				gameSimulationResultRow.publicOpinion = gameSession.publicOpinion;
				gameSimulationResultRow.vaccine = gameSession.vaccineDevelopment;
				gameSimulationResultRow.activeStoriesId = gameSession.activeGameStories.ToArray();

				if (gameSession.HasPlayerWon())
				{
					gameOver = true;
					winRuns++;

					gameSimulationResult.winResult = "VACCINE";
					gameSimulationResult.AddRow(gameSimulationResultRow);

					if (gameSession.day < winDaysRecord)
					{
						winDaysRecord = gameSession.day;
					}

					epochWinDays += gameSession.day;

					continue;
				}

				if (gameSession.HasPlayerLose())
				{
					gameOver = true;
					loseRuns++;
					epochLoseDays += gameSession.day;

					if (gameSession.HasPlayerLoseDueCapacity())
					{
						gameSimulationResult.loseResult = "CAPACITY";
						capacityLoseCount++;
					}
					else if (gameSession.HasPlayerLoseDueMoney())
					{
						gameSimulationResult.loseResult = "MONEY";
						moneyLoseCount++;
					}
					else if (gameSession.HasPlayerLoseDuePublicOpinion())
					{
						gameSimulationResult.loseResult = "PUBLIC OPINION";
						publicOpinionLoseCount++;
					}

					gameSimulationResult.AddRow(gameSimulationResultRow);
					continue;
				}

				if (gameSession.IsCurrentPhaseFinished())
				{
					int nextPhaseId = gameSession.gamePhase.GetNextPhaseId();

					if (nextPhaseId == GameSession.TUTORIAL_GAME_PHASE_ID)
					{
						gameSession.gamePhase = new TutorialGamePhase();
					}
					else
					{
						gameSession.gamePhase = new GamePhase();
					}

					gameSession.gamePhase.Start(gameSession, nextPhaseId, gameSession.day);

					gameSimulationResultRow.phase = nextPhaseId;
				}

				gameSession.advisors = PickAdvisors();
				gameSimulationResultRow.advisorsAvailable = gameSession.advisors.ToArray();

				selectedAdvisorXmlModel = currentStrategy.ChoseAdvisor(gameSession.advisors);
				gameSimulationResultRow.chosenAdvisor = selectedAdvisorXmlModel.name.Replace("_Name", "");
				gameSimulationResultRow.strategyUsed = currentStrategy.GetLogDescription();
				gameSession.DiscardAdvisors();

				List<SuggestionXmlModel> suggestionXmlModels = XmlModelManager.instance.FindModels<SuggestionXmlModel>((suggestionXmlModel) => suggestionXmlModel.advisorId == selectedAdvisorXmlModel.id);
				SuggestionXmlModel selectedSuggestionXmlModel = localPlayer.gameSession.PickNextAvailableSuggestion(selectedAdvisorXmlModel, localPlayer);

				if (selectedSuggestionXmlModel == null)
				{
					gameSimulationResult.AddRow(gameSimulationResultRow);
					gameSimulationResult.AddError("No suggestion available for advisor " + selectedAdvisorXmlModel.name);
					gameOver = true;
					break;
				}

				gameSimulationResultRow.chosenSuggestionId = selectedSuggestionXmlModel.id;

				SuggestionOptionXmlModel selectedSuggestionOptionXmlModel = currentOptionSelectionStrategy.ChoseOption(selectedAdvisorXmlModel, selectedSuggestionXmlModel.suggestionOptionsList);
				gameSimulationResultRow.optionStrategyUsed = currentOptionSelectionStrategy.GetLogDescription();

				gameSimulationResultRow.chosenOptionId = selectedSuggestionOptionXmlModel.id;
				gameSimulationResultRow.startStoryId = selectedSuggestionOptionXmlModel.GetStartStoryId();
				gameSimulationResultRow.stopStoryId = selectedSuggestionOptionXmlModel.GetStopStoryId();

				gameSession.ApplySuggestionOption(selectedSuggestionOptionXmlModel);

				gameSession.UpdateNextDayValues();
				gameSession.NextDay();

				gameSimulationResult.AddRow(gameSimulationResultRow);
			}


			gameSimulationResult.days = localPlayer.gameSession.day;
			gameSimulationResult.Write(writer);
		}

		writer.Close();
	}

	private void InitializePlayer()
	{
		localPlayer = new LocalPlayer();
		localPlayer.Init();
		localPlayer.gameSession = new GameSession();
		localPlayer.gameSession.Initialize(localPlayer);
		localPlayer.gameSession.gamePhase = new TutorialGamePhase();
		localPlayer.gameSession.gamePhase.Start(localPlayer.gameSession, GameSession.TUTORIAL_GAME_PHASE_ID, localPlayer.gameSession.day);
		localPlayer.gameSession.advisors = localPlayer.gameSession.gamePhase.GetAdvisorSpawnPolicy().GetAdvisors();
	}

	public List<AdvisorXmlModel> PickAdvisors()
	{
		previousSelectedAdvisorXmlModel = selectedAdvisorXmlModel;
		selectedAdvisorXmlModel = null;

		List<AdvisorXmlModel> advisorsToAvoid = new List<AdvisorXmlModel>();
		advisorsToAvoid.Add(previousSelectedAdvisorXmlModel);
		return advisorSpawnPolicy.GetAdvisors(advisorsToAvoid);
	}
}
using System.Collections.Generic;
using System.Text;
using System.IO;

public class GameSessionSimulator
{
	public static string SIMULATION_FILE_PATH = "Assets/10_Sandbox/simulations/SimulationResults.csv";

	private int simulationRuns;

	private LocalPlayer localPlayer;

	private IAdvisorSpawnPolicy advisorSpawnPolicy;
	private AdvisorXmlModel selectedAdvisorXmlModel;
	private AdvisorXmlModel previousSelectedAdvisorXmlModel;

	private IGameSimulatorStrategy currentStrategy;
	private List<GameSimulatorStrategyData> strategiesData;

	private ShuffleBag<IGameSimulatorStrategy> strategiesShuffleBag;


	public void Initialize(int simulationRuns, List<GameSimulatorStrategyData> strategiesData)
	{
		this.simulationRuns = simulationRuns;
		this.strategiesData = strategiesData;

		strategiesShuffleBag = new ShuffleBag<IGameSimulatorStrategy>(100);

		advisorSpawnPolicy = new AdvisorRandomSpawnPolicy();
		advisorSpawnPolicy.Initialize();
		selectedAdvisorXmlModel = null;
		previousSelectedAdvisorXmlModel = null;
	}

	private void InitializeStrategiesShuffleBag()
	{
		strategiesShuffleBag.Clear();
		foreach (GameSimulatorStrategyData strategyData in strategiesData)
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

	public void Run()
	{
		StreamWriter writer = new StreamWriter(SIMULATION_FILE_PATH, false);

		for (int i = 0; i < simulationRuns; i++)
		{
			InitializePlayer();

			bool gameOver = false;

			GameSimulationResult gameSimulationResult = new GameSimulationResult();
			gameSimulationResult.run = i + 1;

			InitializeStrategiesShuffleBag();

			while (!gameOver)
			{
				IGameSimulatorStrategy currentStrategy = strategiesShuffleBag.Next();
				GameSession gameSession = localPlayer.gameSession;

				GameSimulationResultRow gameSimulationResultRow = new GameSimulationResultRow();
				gameSimulationResultRow.day = gameSession.day;
				gameSimulationResultRow.phase = gameSession.gamePhase.GetPhaseId();
				gameSimulationResultRow.capacity = gameSession.capacity;
				gameSimulationResultRow.growthRate = gameSession.growthRate;
				gameSimulationResultRow.patients = gameSession.patients[gameSession.day - 1];
				gameSimulationResultRow.money = gameSession.money;
				gameSimulationResultRow.publicOpinion = gameSession.publicOpinion;
				gameSimulationResultRow.vaccine = gameSession.vaccineDevelopment;
				gameSimulationResultRow.activeStoriesId = gameSession.activeGameStories.ToArray();

				if (gameSession.HasPlayerWon())
				{
					gameOver = true;

					gameSimulationResult.winResult = "VACCINE";
					gameSimulationResult.AddRow(gameSimulationResultRow);
					continue;
				}

				if (gameSession.HasPlayerLose())
				{
					gameOver = true;

					if (gameSession.HasPlayerLoseDueCapacity())
					{
						gameSimulationResult.loseResult = "CAPACITY";
					}
					else if (gameSession.HasPlayerLoseDueMoney())
					{
						gameSimulationResult.loseResult = "MONEY";
					}
					else if (gameSession.HasPlayerLoseDuePublicOpinion())
					{
						gameSimulationResult.loseResult = "PUBLIC OPINION";
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
				selectedAdvisorXmlModel = currentStrategy.ChoseAdvisor(gameSession.advisors);
				gameSimulationResultRow.chosenAdvisor = selectedAdvisorXmlModel.name.Replace("_Name", "");
				gameSimulationResultRow.strategyUsed = currentStrategy.GetName();
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

				int randomOptionIndex = RandomGenerator.GetRandom(0, selectedSuggestionXmlModel.suggestionOptionsList.Count);
				SuggestionOptionXmlModel selectedSuggestionOptionXmlModel = selectedSuggestionXmlModel.suggestionOptionsList[randomOptionIndex];

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
		localPlayer.gameSession.Initialize();
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
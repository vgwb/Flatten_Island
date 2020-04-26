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


	public void Initialize(int simulationRuns)
	{
		this.simulationRuns = simulationRuns;

		advisorSpawnPolicy = new AdvisorRandomSpawnPolicy();
		advisorSpawnPolicy.Initialize();
		selectedAdvisorXmlModel = null;
		previousSelectedAdvisorXmlModel = null;
	}

	public void Run()
	{
		StreamWriter writer = new StreamWriter(SIMULATION_FILE_PATH, false);

		for (int i = 0; i < simulationRuns; i++)
		{
			InitializePlayer();

			bool gameOver = false;
			bool runWin = false;

			GameSimulationResult gameSimulationResult = new GameSimulationResult();
			gameSimulationResult.run = i + 1;

			while (!gameOver)
			{
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
					runWin = true;

					gameSimulationResult.AddRow(gameSimulationResultRow);
					continue;
				}

				if (gameSession.HasPlayerLose())
				{
					gameOver = true;
					runWin = false;

					gameSimulationResult.AddRow(gameSimulationResultRow);
					continue;
				}

				if (gameSession.IsCurrentPhaseFinished())
				{
					int nextPhaseId = gameSession.gamePhase.GetNextPhaseId();
					gameSession.gamePhase = new GamePhase();
					gameSession.gamePhase.Start(nextPhaseId, gameSession.day);

					gameSimulationResultRow.phase = nextPhaseId;
				}

				gameSession.advisors = PickAdvisors();
				int randomAdvisorIndex = RandomGenerator.GetRandom(0, gameSession.advisors.Count);
				selectedAdvisorXmlModel = gameSession.advisors[randomAdvisorIndex];
				gameSimulationResultRow.chosenAdvisorId = selectedAdvisorXmlModel.id;

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
			if (runWin)
			{
				gameSimulationResult.result = "WON";
			}
			else
			{
				gameSimulationResult.result = "LOSE";
			}

			gameSimulationResult.Write(writer);
		}

		writer.Close();
	}

	private void InitializePlayer()
	{
		localPlayer = new LocalPlayer();
		localPlayer.Init();
		localPlayer.gameSession = new GameSession();
		List<AdvisorXmlModel> initialAdvisors = PickAdvisors();
		localPlayer.gameSession.Initialize(initialAdvisors);
		localPlayer.gameSession.gamePhase = new GamePhase();
		localPlayer.gameSession.gamePhase.Start(GameSession.INITIAL_PHASE_ID, localPlayer.gameSession.day);
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
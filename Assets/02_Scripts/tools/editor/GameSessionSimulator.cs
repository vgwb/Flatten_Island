using System.Collections.Generic;
using System.Text;

public class GameSessionSimulator
{
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
		StringBuilder resultString = new StringBuilder();

		for (int i = 0; i < simulationRuns; i++)
		{
			localPlayer = new LocalPlayer();
			localPlayer.Init();
			localPlayer.gameSession = new GameSession();
			List<AdvisorXmlModel> initialAdvisors = PickAdvisors();
			localPlayer.gameSession.Initialize(initialAdvisors);
			localPlayer.gameSession.gamePhase = new GamePhase();
			localPlayer.gameSession.gamePhase.Start(GameSession.INITIAL_PHASE_ID, localPlayer.gameSession.day);

			bool gameOver = false;
			bool runWin = false;

			while (!gameOver)
			{
				GameSession gameSession = localPlayer.gameSession;

				if (gameSession.HasPlayerWon())
				{
					gameOver = true;
					runWin = true;
					continue;
				}

				if (gameSession.HasPlayerLose())
				{
					gameOver = true;
					runWin = false;
					continue;
				}

				if (gameSession.IsCurrentPhaseFinished())
				{
					int nextPhaseId = gameSession.gamePhase.GetNextPhaseId();
					gameSession.gamePhase = new GamePhase();
					gameSession.gamePhase.Start(nextPhaseId, gameSession.day);
				}

				gameSession.advisors = PickAdvisors();
				int randomAdvisorIndex = RandomGenerator.GetRandom(0, gameSession.advisors.Count);
				selectedAdvisorXmlModel = gameSession.advisors[randomAdvisorIndex];

				List<SuggestionXmlModel> suggestionXmlModels = XmlModelManager.instance.FindModels<SuggestionXmlModel>((suggestionXmlModel) => suggestionXmlModel.advisorId == selectedAdvisorXmlModel.id);
				SuggestionXmlModel selectedSuggestionXmlModel = PickNextAvailableSuggestion(suggestionXmlModels, selectedAdvisorXmlModel);

				int randomOptionIndex = RandomGenerator.GetRandom(0, selectedSuggestionXmlModel.suggestionOptionsList.Count);
				SuggestionOptionXmlModel selectedSuggestionOptionXmlModel = selectedSuggestionXmlModel.suggestionOptionsList[randomOptionIndex];

				gameSession.ApplySuggestionOption(selectedSuggestionOptionXmlModel);
				gameSession.NextDay();
			}

			resultString.AppendLine("Simulation Over N." + i);
			if (runWin)
			{
				resultString.AppendLine("Result: WON");
			}
			else
			{
				resultString.AppendLine("Result: LOSE");
			}

			resultString.AppendLine(localPlayer.gameSession.StatusString());
			resultString.AppendLine("");
		}

		UnityEngine.Debug.Log(resultString.ToString());
	}

	public List<AdvisorXmlModel> PickAdvisors()
	{
		previousSelectedAdvisorXmlModel = selectedAdvisorXmlModel;
		selectedAdvisorXmlModel = null;

		List<AdvisorXmlModel> advisorsToAvoid = new List<AdvisorXmlModel>();
		advisorsToAvoid.Add(previousSelectedAdvisorXmlModel);
		return advisorSpawnPolicy.GetAdvisors(advisorsToAvoid);
	}

	private SuggestionXmlModel PickNextAvailableSuggestion(List<SuggestionXmlModel> suggestionXmlModels, AdvisorXmlModel advisorXmlModel)
	{
		SuggestionXmlModel selectedSuggestionXmlModel = null;

		//[TEMP] this should be a proper random
		foreach (SuggestionXmlModel suggestionXmlModel in suggestionXmlModels)
		{
			if (suggestionXmlModel.IsAvailable(localPlayer))
			{
				selectedSuggestionXmlModel = suggestionXmlModel;
			}
		}

		//[TEMP] it should never happen in the final game with the full set of suggestions
		if (selectedSuggestionXmlModel == null)
		{
			selectedSuggestionXmlModel = suggestionXmlModels[0];
		}

		return selectedSuggestionXmlModel;
	}
}
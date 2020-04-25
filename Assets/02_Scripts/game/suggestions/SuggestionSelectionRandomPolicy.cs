using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class SuggestionSelectionRandomPolicy : ISuggestionSelectionPolicy
{
	private Dictionary<AdvisorXmlModel, ShuffleBag<SuggestionXmlModel>> advisorSuggestionDictionary;
	private LocalPlayer localPlayer;


	public void Initialize(LocalPlayer localPlayer)
	{
		this.localPlayer = localPlayer;
	}

	public void Reset()
	{
		advisorSuggestionDictionary = new Dictionary<AdvisorXmlModel, ShuffleBag<SuggestionXmlModel>>();

		List<AdvisorXmlModel> advisorXmlModels = XmlModelManager.instance.FindModels<AdvisorXmlModel>();

		foreach (AdvisorXmlModel advisorXmlModel in advisorXmlModels)
		{
			List<SuggestionXmlModel> suggestionXmlModels = XmlModelManager.instance.FindModels<SuggestionXmlModel>((suggestionXmlModel) => suggestionXmlModel.advisorId == advisorXmlModel.id);
			ShuffleBag<SuggestionXmlModel> suggestionsShuffleBag = new ShuffleBag<SuggestionXmlModel>(suggestionXmlModels.Count);
			foreach (SuggestionXmlModel suggestionXmlModel in suggestionXmlModels)
			{
				suggestionsShuffleBag.Add(suggestionXmlModel, 1);
			}

			suggestionsShuffleBag.Shuffle();

			advisorSuggestionDictionary.Add(advisorXmlModel, suggestionsShuffleBag);
		}
	}

	public SuggestionXmlModel GetSuggestion(AdvisorXmlModel advisorXmlModel)
	{
		SuggestionXmlModel selectedSuggestionXmlModel = null;

		int bagEmptyCount = 0;

		while (selectedSuggestionXmlModel == null && bagEmptyCount < 2)
		{
			//could be improved to avoid removing a suggestion that can't be used because requirements are not satisfied
			SuggestionXmlModel randomSuggestionXmlModel = advisorSuggestionDictionary[advisorXmlModel].Next();

			if (randomSuggestionXmlModel.IsAvailable(localPlayer))
			{
				selectedSuggestionXmlModel = randomSuggestionXmlModel;
			}
			else
			{
				if (advisorSuggestionDictionary[advisorXmlModel].IsBagEmpty())
				{
					bagEmptyCount++;
					Debug.LogWarning("No available suggestion for Advisor:" + advisorXmlModel.name + " --> re-shuffling");
				}
			}
		}

		if (selectedSuggestionXmlModel == null)
		{
			Debug.LogError(ReturnNoAvailableSuggestionErrorLog(advisorXmlModel, localPlayer));
		}

		return selectedSuggestionXmlModel;
	}

	private string ReturnNoAvailableSuggestionErrorLog(AdvisorXmlModel advisorXmlModel, LocalPlayer localPlayer)
	{
		GameSession gameSession = localPlayer.gameSession;

		StringBuilder stringBuilder = new StringBuilder("No available suggestion for Advisor:" + advisorXmlModel.name + "\n");
		stringBuilder.AppendLine("Game Phase -->" + " ID:" + gameSession.gamePhase.GetPhaseId() + " Name:" + gameSession.gamePhase.GetName());
		stringBuilder.AppendLine("Active Game Stories: [");

		foreach (GameStoryXmlModel gameStoryXmlModel in gameSession.activeGameStories)
		{
			stringBuilder.Append(gameStoryXmlModel.id + ";");
		}
		stringBuilder.AppendLine("]");

		List<SuggestionXmlModel> suggestionXmlModels = XmlModelManager.instance.FindModels<SuggestionXmlModel>((suggestionXmlModel) => suggestionXmlModel.advisorId == advisorXmlModel.id);
		stringBuilder.AppendLine("Advisor Suggestions:");
		foreach (SuggestionXmlModel suggestionXmlModel in suggestionXmlModels)
		{
			stringBuilder.AppendLine(">> ID:" + suggestionXmlModel.id + " Title:" + suggestionXmlModel.title + " Is Available Now:" + suggestionXmlModel.IsAvailable(localPlayer));
		}

		return stringBuilder.ToString();
	}
}
using System.Collections.Generic;
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

		while (selectedSuggestionXmlModel == null)
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
					Debug.LogWarning("No available suggestion for Advisor:" + advisorXmlModel.name + " --> re-shuffling");
				}
			}
		}

		return selectedSuggestionXmlModel;
	}
}
using UnityEngine;
using System.Collections.Generic;

public class AdvisorsManager : MonoSingleton
{
	public static AdvisorsManager instance
	{
		get
		{
			return GetInstance<AdvisorsManager>();
		}
	}

	public AdvisorsMenu advisorMenu;
	public SuggestionMenu suggestionMenu;

	private LocalPlayer localPlayer;
	private IAdvisorSpawnPolicy advisorSpawnPolicy;

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();
		localPlayer = GameManager.instance.localPlayer;
		advisorSpawnPolicy = new AdvisorRandomSpawnPolicy();
		advisorSpawnPolicy.Initialize();
	}

	public List<AdvisorXmlModel> PickAdvisors()
	{
		return advisorSpawnPolicy.GetAdvisors();
	}

	public void ShowAdvisors(List<AdvisorXmlModel> advisors)
	{
		advisorMenu.Show(advisors);
	}

	public void ShowAdvisorSuggestion(AdvisorXmlModel advisorXmlModel)
	{
		int advisorId = advisorXmlModel.id;
		List<SuggestionXmlModel> suggestionXmlModels = XmlModelManager.instance.FindModels<SuggestionXmlModel>((suggestionXmlModel) => suggestionXmlModel.advisorId == advisorId);

		ShowNextAvailableSuggestion(suggestionXmlModels, advisorXmlModel);
	}

	private void ShowNextAvailableSuggestion(List<SuggestionXmlModel> suggestionXmlModels, AdvisorXmlModel advisorXmlModel)
	{
		SuggestionXmlModel selectedSuggestionXmlModel = null;

		//[TEMP] this should be a proper random
		foreach (SuggestionXmlModel suggestionXmlModel in suggestionXmlModels)
		{
			if (suggestionXmlModel.IsAvailable(localPlayer.gameSession))
			{
				selectedSuggestionXmlModel = suggestionXmlModel;
			}
		}

		//[TEMP] it should never happen in the final game with the full set of suggestions
		if (selectedSuggestionXmlModel == null)
		{
			selectedSuggestionXmlModel = suggestionXmlModels[0];
		}

		suggestionMenu.ShowSuggestion(suggestionXmlModels[0], advisorXmlModel);
	}

	public void ShowAdvisorSuggestionResult(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		suggestionMenu.ShowSuggestionResult(suggestionOptionXmlModel);
	}
}
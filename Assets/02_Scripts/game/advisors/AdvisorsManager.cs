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

	private List<AdvisorXmlModel> currentAdvisors;

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();
		localPlayer = GameManager.instance.localPlayer;
		currentAdvisors = new List<AdvisorXmlModel>();
		advisorSpawnPolicy = new AdvisorRandomSpawnPolicy();
		advisorSpawnPolicy.Initialize();

		//should check here if we have to load advisors from save game
	}

	public List<AdvisorXmlModel> GetCurrentAdvisors()
	{
		return currentAdvisors;
	}

	public void DiscardCurrentAdvisors()
	{
		currentAdvisors.Clear();
	}

	public void ShowAdvisors()
	{
		if (currentAdvisors.Count == 0)
		{
			currentAdvisors = new List<AdvisorXmlModel>(advisorSpawnPolicy.GetAdvisors());
		}

		advisorMenu.Show(currentAdvisors);
	}

	public void ShowAdvisorSuggestion(AdvisorEntry advisorEntrySelected)
	{
		int advisorId = advisorEntrySelected.advisorXmlModel.id;
		List<SuggestionXmlModel> suggestionXmlModels = XmlModelManager.instance.FindModels<SuggestionXmlModel>((suggestionXmlModel) => suggestionXmlModel.advisorId == advisorId);

		//should randomize here
		suggestionMenu.ShowSuggestion(suggestionXmlModels[0], advisorEntrySelected.advisorXmlModel);
	}
}
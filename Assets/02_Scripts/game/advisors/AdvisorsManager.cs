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

	public void OnAdvisorSelected(AdvisorEntry advisorEntrySelected)
	{
		int advisorId = advisorEntrySelected.advisorXmlModel.id;
		List<SuggestionXmlModel> suggestionXmlModels = XmlModelManager.instance.FindModels<SuggestionXmlModel>((suggestionXmlModel) => suggestionXmlModel.advisorId == advisorId);

		//should randomize here
		CreateSuggestionEntry(suggestionXmlModels[0], advisorEntrySelected.advisorXmlModel);
	}

	private SuggestionEntry CreateSuggestionEntry(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		GameObject suggestionEntry = GameObjectFactory.instance.InstantiateGameObject(SuggestionEntry.PREFAB, advisorMenu.transform, false);
		suggestionEntry.gameObject.transform.SetParent(advisorMenu.transform, true);
		SuggestionEntry suggestionEntryScript = suggestionEntry.GetComponent<SuggestionEntry>();
		suggestionEntryScript.SetSuggestion(suggestionXmlModel, advisorXmlModel);
		suggestionEntry.gameObject.SetActive(true);
		return suggestionEntryScript;

	}
}
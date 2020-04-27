using UnityEngine;
using System.Collections.Generic;
using Messages;

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
	public TutorialMenu tutorialMenu;

	private LocalPlayer localPlayer;
	private AdvisorXmlModel selectedAdvisorXmlModel;
	private AdvisorXmlModel previousSelectedAdvisorXmlModel;

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();
		localPlayer = GameManager.instance.localPlayer;
		selectedAdvisorXmlModel = null;
		previousSelectedAdvisorXmlModel = null;

		EventMessageHandler advisorSelectedMessageHandler = new EventMessageHandler(this, OnAdvisorSelected);
		EventMessageManager.instance.AddHandler(typeof(AdvisorSelectedEvent).Name, advisorSelectedMessageHandler);

		EventMessageHandler advisorPresentationExitCompletedMessageHandler = new EventMessageHandler(this, OnAdvisorPresentionExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(AdvisorPresentationExitCompletedEvent).Name, advisorPresentationExitCompletedMessageHandler);
	}

	protected override void OnMonoSingletonDestroyed()
	{
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorSelectedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorPresentationExitCompletedEvent).Name, this);
		base.OnMonoSingletonDestroyed();
	}

	public List<AdvisorXmlModel> PickAdvisors()
	{
		previousSelectedAdvisorXmlModel = selectedAdvisorXmlModel;
		selectedAdvisorXmlModel = null;

		List<AdvisorXmlModel> advisorsToAvoid = new List<AdvisorXmlModel>();
		advisorsToAvoid.Add(previousSelectedAdvisorXmlModel);
		return localPlayer.gameSession.gamePhase.GetAdvisorSpawnPolicy().GetAdvisors(advisorsToAvoid);
	}

	private void OnAdvisorSelected(EventMessage eventMessage)
	{
		AdvisorSelectedEvent advisorSelectedEvent = eventMessage.eventObject as AdvisorSelectedEvent;
		selectedAdvisorXmlModel = advisorSelectedEvent.advisorEntrySelected.advisorXmlModel;
	}

	private void OnAdvisorPresentionExitCompleted(EventMessage eventMessage)
	{
		AdvisorPresentationExitCompletedEvent advisorPresentationExitCompleted = eventMessage.eventObject as AdvisorPresentationExitCompletedEvent;
		selectedAdvisorXmlModel = advisorPresentationExitCompleted.advisorPresentation.advisorXmlModel;
	}

	public void ShowAdvisors(List<AdvisorXmlModel> advisors)
	{
		advisorMenu.Show(advisors);
	}

	public void ShowAdvisorPresentation(AdvisorXmlModel advisor)
	{
		tutorialMenu.Show(advisor);
	}

	public void ShowAdvisorSuggestion()
	{
		SuggestionXmlModel selectedSuggestionXmlModel = localPlayer.gameSession.PickNextAvailableSuggestion(selectedAdvisorXmlModel, localPlayer);
		suggestionMenu.ShowSuggestion(selectedSuggestionXmlModel, selectedAdvisorXmlModel);
	}

	public void ShowAdvisorSuggestionResult(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		suggestionMenu.ShowSuggestionResult(suggestionOptionXmlModel);
	}
}
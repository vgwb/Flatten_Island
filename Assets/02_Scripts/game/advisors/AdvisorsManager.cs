﻿using UnityEngine;
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

		EventMessageHandler tutorialDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnTutorialDialogExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(TutorialDialogExitCompletedEvent).Name, tutorialDialogExitCompletedMessageHandler);
	}

	protected override void OnMonoSingletonDestroyed()
	{
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorSelectedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(TutorialDialogExitCompletedEvent).Name, this);
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

	private void OnTutorialDialogExitCompleted(EventMessage eventMessage)
	{
		TutorialDialogExitCompletedEvent tutorialDialogExitCompleted = eventMessage.eventObject as TutorialDialogExitCompletedEvent;
		selectedAdvisorXmlModel = tutorialDialogExitCompleted.tutorialDialog.advisorXmlModel;
	}

	public void ShowAdvisors(List<AdvisorXmlModel> advisors)
	{
		advisorMenu.Show(advisors);
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
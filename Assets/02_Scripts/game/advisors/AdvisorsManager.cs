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
	private IAdvisorSpawnPolicy advisorSpawnPolicy;
	private AdvisorXmlModel selectedAdvisorXmlModel;
	private AdvisorXmlModel previousSelectedAdvisorXmlModel;

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();
		localPlayer = GameManager.instance.localPlayer;
		advisorSpawnPolicy = new AdvisorRandomSpawnPolicy();
		advisorSpawnPolicy.Initialize();
		selectedAdvisorXmlModel = null;
		previousSelectedAdvisorXmlModel = null;

		EventMessageHandler advisorSelectedMessageHandler = new EventMessageHandler(this, OnAdvisorSelected);
		EventMessageManager.instance.AddHandler(typeof(AdvisorSelectedEvent).Name, advisorSelectedMessageHandler);
	}

	protected override void OnMonoSingletonDestroyed()
	{
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorSelectedEvent).Name, this);
		base.OnMonoSingletonDestroyed();
	}

	public List<AdvisorXmlModel> PickAdvisors()
	{
		previousSelectedAdvisorXmlModel = selectedAdvisorXmlModel;
		selectedAdvisorXmlModel = null;

		List<AdvisorXmlModel> advisorsToAvoid = new List<AdvisorXmlModel>();
		advisorsToAvoid.Add(previousSelectedAdvisorXmlModel);
		return advisorSpawnPolicy.GetAdvisors(advisorsToAvoid);
	}

	private void OnAdvisorSelected(EventMessage eventMessage)
	{
		AdvisorSelectedEvent advisorSelectedEvent = eventMessage.eventObject as AdvisorSelectedEvent;
		selectedAdvisorXmlModel = advisorSelectedEvent.advisorEntrySelected.advisorXmlModel;
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
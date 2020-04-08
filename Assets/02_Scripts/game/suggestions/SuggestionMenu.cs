using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class SuggestionMenu : MonoBehaviour
{
	private SuggestionEntry suggestionEntry;
	private SuggestionResultEntry suggestionResultEntry;
	private AdvisorXmlModel advisorXmlModel;
	private SuggestionOptionXmlModel selectedSuggestionOptionXmlModel;

	private void Awake()
	{
		suggestionEntry = null;
		suggestionResultEntry = null;
		selectedSuggestionOptionXmlModel = null;
		advisorXmlModel = null;
	}

	private void OnEnable()
	{
		EventMessageHandler suggestionOptionSelectedMessageHandler = new EventMessageHandler(this, OnSuggestionOptionSelectedEvent);
		EventMessageManager.instance.AddHandler(typeof(SuggestionOptionSelectedEvent).Name, suggestionOptionSelectedMessageHandler);

		EventMessageHandler suggestionEntryEnterCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionEntryEnterCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionEntryEnterCompletedEvent).Name, suggestionEntryEnterCompletedMessageHandler);

		EventMessageHandler suggestionEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionEntryExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionEntryExitCompletedEvent).Name, suggestionEntryExitCompletedMessageHandler);

		EventMessageHandler suggestionResultEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionResultEntryExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, suggestionResultEntryExitCompletedMessageHandler);
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionOptionSelectedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionEntryEnterCompletedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionEntryExitCompletedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, this);
	}

	public void ShowSuggestion(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		this.advisorXmlModel = advisorXmlModel;
		suggestionEntry = CreateSuggestionEntry(suggestionXmlModel, advisorXmlModel);
		suggestionEntry.PlayEnterRecipe();
	}

	private void OnSuggestionOptionSelectedEvent(EventMessage eventMessage)
	{
		//should disable Input
		SuggestionOptionSelectedEvent suggestionOptionSelectedEvent = eventMessage.eventObject as SuggestionOptionSelectedEvent;
		selectedSuggestionOptionXmlModel = suggestionOptionSelectedEvent.suggestionOptionXmlModel;
	}

	private SuggestionEntry CreateSuggestionEntry(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		GameObject suggestionEntry = GameObjectFactory.instance.InstantiateGameObject(SuggestionEntry.PREFAB, this.transform, false);
		suggestionEntry.gameObject.transform.SetParent(this.transform, true);
		SuggestionEntry suggestionEntryScript = suggestionEntry.GetComponent<SuggestionEntry>();
		suggestionEntryScript.SetSuggestion(suggestionXmlModel, advisorXmlModel);
		suggestionEntry.gameObject.SetActive(true);
		return suggestionEntryScript;
	}

	private void OnSuggestionEntryEnterCompleted(EventMessage eventMessage)
	{
		AdvisorEntryEnterCompletedEvent adviceEntryEnterCompletedEvent = eventMessage.eventObject as AdvisorEntryEnterCompletedEvent;
	}

	private void OnSuggestionEntryExitCompleted(EventMessage eventMessage)
	{
		SuggestionEntryExitCompletedEvent suggestionEntryExitCompletedEvent = eventMessage.eventObject as SuggestionEntryExitCompletedEvent;
		GameObjectFactory.instance.ReleaseGameObject(suggestionEntry.gameObject, SuggestionEntry.PREFAB);

		Debug.Log("Suggestion Entry has exited");

		ShowSuggestionResult();
	}

	public void ShowSuggestionResult()
	{
		suggestionResultEntry = CreateSuggestionResultEntry(selectedSuggestionOptionXmlModel);
		suggestionResultEntry.PlayEnterRecipe();
	}

	private SuggestionResultEntry CreateSuggestionResultEntry(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		GameObject suggestionResultEntry = GameObjectFactory.instance.InstantiateGameObject(SuggestionResultEntry.PREFAB, this.transform, false);
		suggestionResultEntry.gameObject.transform.SetParent(this.transform, true);
		SuggestionResultEntry suggestionResultEntryScript = suggestionResultEntry.GetComponent<SuggestionResultEntry>();
		suggestionResultEntryScript.SetSuggestionResult(suggestionOptionXmlModel, advisorXmlModel);
		suggestionResultEntry.gameObject.SetActive(true);
		return suggestionResultEntryScript;
	}

	private void OnSuggestionResultEntryExitCompleted(EventMessage eventMessage)
	{
		SuggestionResultEntryExitCompletedEvent suggestionResultEntryExitCompletedEvent = eventMessage.eventObject as SuggestionResultEntryExitCompletedEvent;
		GameObjectFactory.instance.ReleaseGameObject(suggestionResultEntry.gameObject, SuggestionResultEntry.PREFAB);

		Debug.Log("Suggestion Entry has exited");

		suggestionResultEntry = null;
		suggestionEntry = null;
		selectedSuggestionOptionXmlModel = null;
		advisorXmlModel = null;
	}
}

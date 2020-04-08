using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class SuggestionMenu : MonoBehaviour
{
	private SuggestionEntry currentSuggestionEntry;
	private SuggestionOptionXmlModel selectedSuggestionOptionXmlModel;

	private void OnEnable()
	{
		EventMessageHandler suggestionOptionSelectedMessageHandler = new EventMessageHandler(this, OnSuggestionOptionSelectedEvent);
		EventMessageManager.instance.AddHandler(typeof(SuggestionOptionSelectedEvent).Name, suggestionOptionSelectedMessageHandler);

		EventMessageHandler suggestionEntryEnterCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionEntryEnterCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionEntryEnterCompletedEvent).Name, suggestionEntryEnterCompletedMessageHandler);

		EventMessageHandler suggestionEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionEntryExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionEntryExitCompletedEvent).Name, suggestionEntryExitCompletedMessageHandler);
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionOptionSelectedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionEntryEnterCompletedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionEntryExitCompletedEvent).Name, this);
	}

	public void Show(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		currentSuggestionEntry = null;
		currentSuggestionEntry = CreateSuggestionEntry(suggestionXmlModel, advisorXmlModel);
		currentSuggestionEntry.PlayEnterRecipe();
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
		GameObjectFactory.instance.ReleaseGameObject(currentSuggestionEntry.gameObject, SuggestionEntry.PREFAB);

		Debug.Log("Suggestion Entry has exited");

		//GameManager.instance.OnSuggestionSelected(selectedSuggestionOptionXmlModel);
		//should Hide the Suggestion Menu
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Messages;

public class AdvisorsMenu : MonoBehaviour
{
	public GridLayoutGroup gridLayoutGroup;

	private List<AdvisorEntry> currentAdvisorEntries;
	private AdvisorEntry advisorEntrySelected;

	private void OnEnable()
	{
		EventMessageHandler advisorSelectedMessageHandler = new EventMessageHandler(this, OnAdvisorSelected);
		EventMessageManager.instance.AddHandler(typeof(AdvisorSelectedEvent).Name, advisorSelectedMessageHandler);

		EventMessageHandler advisorEntryEnterCompletedMessageHandler = new EventMessageHandler(this, OnAdvisorEntryEnterCompleted);
		EventMessageManager.instance.AddHandler(typeof(AdvisorEntryEnterCompletedEvent).Name, advisorEntryEnterCompletedMessageHandler);

		EventMessageHandler advisorEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnAdvisorEntryExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(AdvisorEntryExitCompletedEvent).Name, advisorEntryExitCompletedMessageHandler);
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorSelectedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorEntryEnterCompletedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorEntryExitCompletedEvent).Name, this);
	}

	public void Show(List<AdvisorXmlModel> advisorXmlModels)
	{
		advisorEntrySelected = null;

		if (currentAdvisorEntries == null)
		{
			currentAdvisorEntries = new List<AdvisorEntry>();
		}
		else
		{
			ReleaseAdvisorEntries();
			currentAdvisorEntries.Clear();
		}

		foreach (AdvisorXmlModel advisorXmlModel in advisorXmlModels)
		{
			AdvisorEntry advisorEntry = CreateAdvisorEntry(advisorXmlModel);
			currentAdvisorEntries.Add(advisorEntry);
			advisorEntry.PlayEnterRecipe();
		}
	}

	private void OnAdvisorSelected(EventMessage eventMessage)
	{
		//should disable Input

		AdvisorSelectedEvent advisorSelectedEvent = eventMessage.eventObject as AdvisorSelectedEvent;
		advisorEntrySelected = advisorSelectedEvent.advisorEntrySelected;
	}

	private AdvisorEntry CreateAdvisorEntry(AdvisorXmlModel advisorXmlModel)
	{
		GameObject advisorEntry = GameObjectFactory.instance.InstantiateGameObject(AdvisorEntry.PREFAB, gridLayoutGroup.transform, false);
		advisorEntry.gameObject.transform.SetParent(gridLayoutGroup.transform, true);
		AdvisorEntry advisorEntryScript = advisorEntry.GetComponent<AdvisorEntry>();
		advisorEntryScript.SetAdvisor(advisorXmlModel);
		advisorEntry.gameObject.SetActive(true);
		return advisorEntryScript;
	}

	private void OnAdvisorEntryEnterCompleted(EventMessage eventMessage)
	{
		AdvisorEntryEnterCompletedEvent adviceEntryEnterCompletedEvent = eventMessage.eventObject as AdvisorEntryEnterCompletedEvent;
	}

	private void OnAdvisorEntryExitCompleted(EventMessage eventMessage)
	{
		AdvisorEntryExitCompletedEvent adviceEntryExitCompletedEvent = eventMessage.eventObject as AdvisorEntryExitCompletedEvent;
		GameObjectFactory.instance.ReleaseGameObject(adviceEntryExitCompletedEvent.advisorEntry.gameObject, AdvisorEntry.PREFAB);
		currentAdvisorEntries.Remove(adviceEntryExitCompletedEvent.advisorEntry);

		if (currentAdvisorEntries.Count == 0)
		{
			Debug.Log("All Advisor Entries have exited");

			AdvisorsManager.instance.ShowAdvisorSuggestion(advisorEntrySelected);

			//should tell AdvisorManager I have finished, passing the selected advisor
			//should Hide the Menu
			//should re-enable Input here o later?
		}
	}

	private void ReleaseAdvisorEntries()
	{
		foreach (AdvisorEntry advisorEntry in currentAdvisorEntries)
		{
			GameObjectFactory.instance.ReleaseGameObject(advisorEntry.gameObject, AdvisorEntry.PREFAB);
		}
	}
}

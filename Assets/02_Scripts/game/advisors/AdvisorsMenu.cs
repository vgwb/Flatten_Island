using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class AdvisorsMenu : MonoBehaviour
{
	public GridLayoutGroup gridLayoutGroup;

	private void OnEnable()
	{
		EventMessageHandler advisorSelectedMessageHandler = new EventMessageHandler(this, OnAdvisorSelected);
		EventMessageManager.instance.AddHandler(typeof(AdvisorSelectedEvent).Name, advisorSelectedMessageHandler);

		GenerateAdvisorEntries();
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorSelectedEvent).Name, this);
	}

	private void OnAdvisorSelected(EventMessage eventMessage)
	{
		AdvisorSelectedEvent advisorSelectedEvent = eventMessage.eventObject as AdvisorSelectedEvent;

		Debug.Log("Selected advisor:" + advisorSelectedEvent.advisorEntrySelected.advisorXmlModel.name);
	}

	public void GenerateAdvisorEntries()
	{
		RemovedAdvisorEntries();

		foreach (AdvisorXmlModel advisorXmlModel in AdvisorsManager.instance.GetCurrentAdvisors())
		{
			CreateAdvisorEntry(advisorXmlModel);
		}
	}

	private void CreateAdvisorEntry(AdvisorXmlModel advisorXmlModel)
	{
		GameObject advisorEntry = GameObjectFactory.instance.InstantiateGameObject(AdvisorEntry.PREFAB, gridLayoutGroup.transform, false);
		advisorEntry.gameObject.transform.SetParent(gridLayoutGroup.transform, true);
		AdvisorEntry advisorEntryScript = advisorEntry.GetComponent<AdvisorEntry>();
		advisorEntryScript.SetAdvisor(advisorXmlModel);
		advisorEntry.gameObject.SetActive(true);
	}

	private void RemovedAdvisorEntries()
	{
		AdvisorEntry[] advisorEntries = gridLayoutGroup.GetComponentsInChildren<AdvisorEntry>();
		foreach (AdvisorEntry advisorEntry in advisorEntries)
		{
			GameObjectFactory.instance.ReleaseGameObject(advisorEntry.gameObject, AdvisorEntry.PREFAB);
		}
	}
}

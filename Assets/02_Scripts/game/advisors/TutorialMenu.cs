using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class TutorialMenu : MonoBehaviour
{
	private AdvisorPresentationDialog advisorPresentationDialog;

	private void OnEnable()
	{
		EventMessageHandler advisorPresentationExitCompletedMessageHandler = new EventMessageHandler(this, OnAdvisorPresentionExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(AdvisorPresentationExitCompletedEvent).Name, advisorPresentationExitCompletedMessageHandler);
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorPresentationExitCompletedEvent).Name, this);
	}

	public void OnAdvisorPresentionExitCompleted(EventMessage eventMessage)
	{
		AdvisorPresentationExitCompletedEvent advisorPresentationExitCompletedEvent = eventMessage.eventObject as AdvisorPresentationExitCompletedEvent;

		if (advisorPresentationDialog.advisorXmlModel == advisorPresentationExitCompletedEvent.advisorPresentation.advisorXmlModel)
		{
			AllAdvisorsExitCompletedEvent allAdvisorsExitCompletedEvent = AllAdvisorsExitCompletedEvent.CreateInstance(advisorPresentationDialog.advisorXmlModel);
			EventMessage allAdvisorsExitCompletedEventMessage = new EventMessage(this, allAdvisorsExitCompletedEvent);
			allAdvisorsExitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
			EventMessageManager.instance.QueueMessage(allAdvisorsExitCompletedEventMessage);

			advisorPresentationDialog.Release();
		}
	}

	public void Show(AdvisorXmlModel advisorXmlModel)
	{
		advisorPresentationDialog = AdvisorPresentationDialog.Show(advisorXmlModel, gameObject.transform);
	}
}

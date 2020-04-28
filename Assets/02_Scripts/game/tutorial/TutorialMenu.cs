using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class TutorialMenu : MonoBehaviour
{
	private TutorialDialog advisorPresentationDialog;
	public Image advisorPortrait;

	private void OnEnable()
	{

		EventMessageHandler advisorPresentationExitCompletedMessageHandler = new EventMessageHandler(this, OnAdvisorPresentionExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(TutorialDialogExitCompletedEvent).Name, advisorPresentationExitCompletedMessageHandler);
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(TutorialDialogExitCompletedEvent).Name, this);
	}

	public void OnAdvisorPresentionExitCompleted(EventMessage eventMessage)
	{
		TutorialDialogExitCompletedEvent advisorPresentationExitCompletedEvent = eventMessage.eventObject as TutorialDialogExitCompletedEvent;

		if (advisorPresentationDialog.advisorXmlModel == advisorPresentationExitCompletedEvent.tutorialDialog.advisorXmlModel)
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
		TryShowAdvisorPortrait(advisorXmlModel);

		advisorPresentationDialog = TutorialDialog.ShowAdvisorPresentation(advisorXmlModel, gameObject.transform);
	}

	private void TryShowAdvisorPortrait(AdvisorXmlModel advisorXmlModel)
	{
		if (!string.IsNullOrEmpty(advisorXmlModel.portraitFullSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.portraitFullSprite);
			advisorPortrait.overrideSprite = advisorSprite;
			advisorPortrait.gameObject.SetActive(true);
		}
		else
		{
			advisorPortrait.gameObject.SetActive(false);
		}
	}

	public void HideAdvisorPortrait()
	{
		advisorPortrait.gameObject.SetActive(false);
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class TutorialMenu : MonoSingleton
{
	public Image advisorPortrait;
	public string tutorialNextDayTextLocalizationId = "Tutorial_NextDay_Text";

	private TutorialDialog tutorialDialog;

	public static TutorialMenu instance
	{
		get
		{
			return GetInstance<TutorialMenu>();
		}
	}

	private void OnEnable()
	{
		EventMessageHandler tutorialDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnTutorialDialogExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(TutorialDialogExitCompletedEvent).Name, tutorialDialogExitCompletedMessageHandler);
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(TutorialDialogExitCompletedEvent).Name, this);
	}

	public void OnTutorialDialogExitCompleted(EventMessage eventMessage)
	{
		TutorialDialogExitCompletedEvent tutorialDialogExitCompletedEvent = eventMessage.eventObject as TutorialDialogExitCompletedEvent;

		if (tutorialDialogExitCompletedEvent.tutorialDialog.advisorXmlModel != null)
		{
			if (tutorialDialog.advisorXmlModel == tutorialDialogExitCompletedEvent.tutorialDialog.advisorXmlModel)
			{
				AllAdvisorsExitCompletedEvent allAdvisorsExitCompletedEvent = AllAdvisorsExitCompletedEvent.CreateInstance(tutorialDialog.advisorXmlModel);
				EventMessage allAdvisorsExitCompletedEventMessage = new EventMessage(this, allAdvisorsExitCompletedEvent);
				allAdvisorsExitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
				EventMessageManager.instance.QueueMessage(allAdvisorsExitCompletedEventMessage);

				tutorialDialog.Release();
			}
		}
	}

	public void ShowAdvisorPresentation(AdvisorXmlModel advisorXmlModel)
	{
		TryShowAdvisorPortrait(advisorXmlModel);

		tutorialDialog = TutorialDialog.ShowAdvisorPresentation(advisorXmlModel, gameObject.transform);
	}

	public void ShowNextDayTipDialog()
	{
		tutorialDialog = TutorialDialog.ShowTutorialTip(tutorialNextDayTextLocalizationId, gameObject.transform);
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

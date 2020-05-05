using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class AdvisorEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/AdvisorEntry";

	public Image portrait;
	public Image background;
	public Text advisorName;
	public AdvisorEntryChef advisorEntryChef;

	public AdvisorXmlModel advisorXmlModel;

	private void OnEnable()
	{
		EventMessageHandler advisorSelectedMessageHandler = new EventMessageHandler(this, OnAdvisorSelectedEvent);
		EventMessageManager.instance.AddHandler(typeof(AdvisorSelectedEvent).Name, advisorSelectedMessageHandler);
	}

	public void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(AdvisorSelectedEvent).Name, this);
	}

	public void SetAdvisor(AdvisorXmlModel advisorXmlModel)
	{
		this.advisorXmlModel = advisorXmlModel;

		LocalizationManager.instance.SetLocalizedText(advisorName, advisorXmlModel.name);
		if (!string.IsNullOrEmpty(advisorXmlModel.portraitSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.portraitSprite);
			portrait.overrideSprite = advisorSprite;
		}

		if (!string.IsNullOrEmpty(advisorXmlModel.portraitBackgroundSprite))
		{
			Sprite advisorBackgroundSprite = Resources.Load<Sprite>(advisorXmlModel.portraitBackgroundSprite);
			background.overrideSprite = advisorBackgroundSprite;
		}
	}

	public void OnSelected()
	{
		//Debug.Log("Advisor " + advisorXmlModel.name + " Selected");

		AdvisorSelectedEvent advisorSelectedEvent = AdvisorSelectedEvent.CreateInstance(this);
		EventMessage advisorSelectedEventMessage = new EventMessage(this, advisorSelectedEvent);
		advisorSelectedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(advisorSelectedEventMessage);
	}

	public void OnAdvisorSelectedEvent(EventMessage eventMessage)
	{
		AdvisorSelectedEvent advisorSelectedEvent = eventMessage.eventObject as AdvisorSelectedEvent;
		if (advisorSelectedEvent.advisorEntrySelected == this)
		{
			advisorEntryChef.Cook(advisorEntryChef.onSelectedRecipe, OnSelectedRecipeCompleted);
		}
		else
		{
			advisorEntryChef.Cook(advisorEntryChef.onDiscardedRecipe, OnDiscardedRecipeCompleted);
		}
	}

	private void OnSelectedRecipeCompleted()
	{
		//Debug.Log("Advisor " + advisorXmlModel.name + " Selected Recipe completed");
		SendExitCompletedEvent();
	}

	public void OnDiscardedRecipeCompleted()
	{
		//Debug.Log("Advisor " + advisorXmlModel.name + " Discarded Recipe completed");
		SendExitCompletedEvent();
	}

	private void SendExitCompletedEvent()
	{
		AdvisorEntryExitCompletedEvent exitCompletedEvent = AdvisorEntryExitCompletedEvent.CreateInstance(this);
		EventMessage exitCompletedEventMessage = new EventMessage(this, exitCompletedEvent);
		exitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(exitCompletedEventMessage);
	}

	private void SendEnterCompletedEvent()
	{
		AdvisorEntryEnterCompletedEvent enterCompletedEvent = AdvisorEntryEnterCompletedEvent.CreateInstance(this);
		EventMessage enterCompletedEventMessage = new EventMessage(this, enterCompletedEvent);
		enterCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(enterCompletedEventMessage);
	}

	public void PlayEnterRecipe(int advisorIndex)
	{
		switch (advisorIndex)
		{
			case 0: advisorEntryChef.Cook(advisorEntryChef.onEnterFirstRecipe, OnEnterRecipeCompleted); break;
			case 1: advisorEntryChef.Cook(advisorEntryChef.onEnterSecondRecipe, OnEnterRecipeCompleted); break;
			case 2: advisorEntryChef.Cook(advisorEntryChef.onEnterThirdRecipe, OnEnterRecipeCompleted); break;
		}
	}

	private void OnEnterRecipeCompleted()
	{
		//Debug.Log("Advisor " + advisorXmlModel.name + " Enter Recipe completed");
		SendEnterCompletedEvent();
	}
}

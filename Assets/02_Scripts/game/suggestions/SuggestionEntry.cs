using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class SuggestionEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/SuggestionEntry";

	public SuggestionEntryChef suggestionEntryChef;
	public Image advisorIcon;
	public Text title;
	public Text description;
	public SuggestionButton buttonOptionA;
	public SuggestionButton buttonOptionB;

	public SuggestionXmlModel suggestionXmlModel;

	private void OnEnable()
	{
		EventMessageHandler SuggestionOptionSelectedMessageHandler = new EventMessageHandler(this, OnSuggestionOptionSelectedEvent);
		EventMessageManager.instance.AddHandler(typeof(SuggestionOptionSelectedEvent).Name, SuggestionOptionSelectedMessageHandler);
	}

	public void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionOptionSelectedEvent).Name, this);
	}

	public void SetSuggestion(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		this.suggestionXmlModel = suggestionXmlModel;

		title.text = LocalizationManager.instance.GetText(suggestionXmlModel.title);
		description.text = LocalizationManager.instance.GetText(suggestionXmlModel.description);

		if (!string.IsNullOrEmpty(advisorXmlModel.iconSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.iconSprite);
			advisorIcon.overrideSprite = advisorSprite;
		}

		SuggestionOptionXmlModel optionAXmlModel = suggestionXmlModel.suggestionOptionsList[0];
		SuggestionOptionXmlModel optionBXmlModel = suggestionXmlModel.suggestionOptionsList[1];

		buttonOptionA.SetButton(optionAXmlModel);
		buttonOptionB.SetButton(optionBXmlModel);
	}

	public void OnButtonOptionASelected()
	{
		Debug.Log("Suggestion - Option A Selected");
		SendSuggestionOptionSelectedMessage(suggestionXmlModel.suggestionOptionsList[0]);
	}

	public void OnButtonOptionBSelected()
	{
		Debug.Log("Suggestion - Option B Selected");
		SendSuggestionOptionSelectedMessage(suggestionXmlModel.suggestionOptionsList[1]);
	}

	public void OnSuggestionOptionSelectedEvent(EventMessage eventMessage)
	{
		SuggestionOptionSelectedEvent suggestionOptionSelectedEvent = eventMessage.eventObject as SuggestionOptionSelectedEvent;
		suggestionEntryChef.Cook(suggestionEntryChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void SendSuggestionOptionSelectedMessage(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		SuggestionOptionSelectedEvent suggestionOptionSelectedEvent = SuggestionOptionSelectedEvent.CreateInstance(suggestionOptionXmlModel);
		EventMessage suggestionOptionSelectedEventMessage = new EventMessage(this, suggestionOptionSelectedEvent);
		suggestionOptionSelectedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(suggestionOptionSelectedEventMessage);
	}

	private void OnExitRecipeCompleted()
	{
		Debug.Log("Suggestion " + suggestionXmlModel.name + " Exit Recipe completed");
		SendExitCompletedEvent();
	}

	private void SendExitCompletedEvent()
	{
		SuggestionEntryExitCompletedEvent exitCompletedEvent = SuggestionEntryExitCompletedEvent.CreateInstance(this);
		EventMessage exitCompletedEventMessage = new EventMessage(this, exitCompletedEvent);
		exitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(exitCompletedEventMessage);
	}

	private void SendEnterCompletedEvent()
	{
		SuggestionEntryEnterCompletedEvent enterCompletedEvent = SuggestionEntryEnterCompletedEvent.CreateInstance(this);
		EventMessage enterCompletedEventMessage = new EventMessage(this, enterCompletedEvent);
		enterCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(enterCompletedEventMessage);
	}

	public void PlayEnterRecipe()
	{
		suggestionEntryChef.Cook(suggestionEntryChef.onEnterRecipe, OnEnterRecipeCompleted);
	}

	private void OnEnterRecipeCompleted()
	{
		Debug.Log("Suggestion " + suggestionXmlModel.name + " Enter Recipe completed");
		SendEnterCompletedEvent();
	}
}

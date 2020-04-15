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
	public Image background;
	public Image advisorBubble;

	public SuggestionXmlModel suggestionXmlModel;
	private SuggestionOptionXmlModel selectedSuggestionOptionXmlModel;

	public void SetSuggestion(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		this.suggestionXmlModel = suggestionXmlModel;
		selectedSuggestionOptionXmlModel = null;

		LocalizationManager.instance.SetLocalizedText(title, suggestionXmlModel.title);
		LocalizationManager.instance.SetLocalizedText(description, suggestionXmlModel.description);

		if (!string.IsNullOrEmpty(advisorXmlModel.iconSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.iconSprite);
			advisorIcon.overrideSprite = advisorSprite;
		}

		if (!string.IsNullOrEmpty(advisorXmlModel.bubbleSprite))
		{
			Sprite bubbleSprite = Resources.Load<Sprite>(advisorXmlModel.bubbleSprite);
			advisorBubble.overrideSprite = bubbleSprite;
		}

		if (!string.IsNullOrEmpty(advisorXmlModel.suggestionBackgroundSprite))
		{
			Sprite suggestionBackgroundSprite = Resources.Load<Sprite>(advisorXmlModel.suggestionBackgroundSprite);
			background.overrideSprite = suggestionBackgroundSprite;
		}

		SuggestionOptionXmlModel optionAXmlModel = suggestionXmlModel.suggestionOptionsList[0];
		SuggestionOptionXmlModel optionBXmlModel = suggestionXmlModel.suggestionOptionsList[1];

		buttonOptionA.SetButton(optionAXmlModel);
		buttonOptionB.SetButton(optionBXmlModel);
	}

	public void OnButtonOptionASelected()
	{
		Debug.Log("Suggestion - Option A Selected");
		selectedSuggestionOptionXmlModel = suggestionXmlModel.suggestionOptionsList[0];
		SendSuggestionOptionSelectedMessage(suggestionXmlModel.suggestionOptionsList[0]);

		suggestionEntryChef.Cook(suggestionEntryChef.onExitRecipe, OnExitRecipeCompleted);
	}

	public void OnButtonOptionBSelected()
	{
		Debug.Log("Suggestion - Option B Selected");
		selectedSuggestionOptionXmlModel = suggestionXmlModel.suggestionOptionsList[1];
		SendSuggestionOptionSelectedMessage(suggestionXmlModel.suggestionOptionsList[1]);

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
		//Debug.Log("Suggestion " + suggestionXmlModel.name + " Exit Recipe completed");
		SendExitCompletedEvent();
	}

	private void SendExitCompletedEvent()
	{
		SuggestionEntryExitCompletedEvent exitCompletedEvent = SuggestionEntryExitCompletedEvent.CreateInstance(selectedSuggestionOptionXmlModel);
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
		//Debug.Log("Suggestion " + suggestionXmlModel.name + " Enter Recipe completed");
		SendEnterCompletedEvent();
	}
}

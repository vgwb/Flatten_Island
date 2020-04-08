using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class SuggestionResultEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/SuggestionResultEntry";

	public SuggestionResultEntryChef suggestionResultEntryChef;
	public Image advisorPortrait;
	public GridLayoutGroup parametersGridLayoutGroup;

	private SuggestionOptionXmlModel suggestionOptionXmlModel;

	private void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	public void SetSuggestionResult(SuggestionOptionXmlModel suggestionOptionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		this.suggestionOptionXmlModel = suggestionOptionXmlModel;

		if (!string.IsNullOrEmpty(advisorXmlModel.portraitSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.portraitSprite);
			advisorPortrait.overrideSprite = advisorSprite;
		}
	}

	public void OnOkButtonSelected()
	{
		suggestionResultEntryChef.Cook(suggestionResultEntryChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void OnExitRecipeCompleted()
	{
		Debug.Log("Suggestion Result Exit Recipe completed");
		SendExitCompletedEvent();
	}

	private void SendExitCompletedEvent()
	{
		SuggestionResultEntryExitCompletedEvent exitCompletedEvent = SuggestionResultEntryExitCompletedEvent.CreateInstance(this);
		EventMessage exitCompletedEventMessage = new EventMessage(this, exitCompletedEvent);
		exitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(exitCompletedEventMessage);
	}

	private void SendEnterCompletedEvent()
	{
		SuggestionResultEntryEnterCompletedEvent enterCompletedEvent = SuggestionResultEntryEnterCompletedEvent.CreateInstance(this);
		EventMessage enterCompletedEventMessage = new EventMessage(this, enterCompletedEvent);
		enterCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(enterCompletedEventMessage);
	}

	public void PlayEnterRecipe()
	{
		suggestionResultEntryChef.Cook(suggestionResultEntryChef.onEnterRecipe, OnEnterRecipeCompleted);
	}

	private void OnEnterRecipeCompleted()
	{
		Debug.Log("Suggestion Result Enter Recipe completed");
		SendEnterCompletedEvent();
	}
}

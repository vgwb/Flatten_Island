using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class SuggestionResultEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/SuggestionResultEntry";

	public Sprite moneySprite;
	public Sprite publicOpinionSprite;
	public Sprite growRateSprite;
	public Sprite capacitySprite;
	public Sprite vaccineSprite;

	public SuggestionResultEntryChef suggestionResultEntryChef;
	public Image advisorPortrait;
	public Image background;
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

		if (!string.IsNullOrEmpty(advisorXmlModel.portraitFullSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.portraitFullSprite);
			advisorPortrait.overrideSprite = advisorSprite;
		}

		if (!string.IsNullOrEmpty(advisorXmlModel.suggestionResultBackgroundSprite))
		{
			Sprite backgroundSprite = Resources.Load<Sprite>(advisorXmlModel.suggestionResultBackgroundSprite);
			background.overrideSprite = backgroundSprite;
		}

		if (suggestionOptionXmlModel.publicOpinionModifier != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(suggestionOptionXmlModel.publicOpinionModifier, publicOpinionSprite);
			optionParameterEntry.gameObject.SetActive(true);
		}

		if (suggestionOptionXmlModel.moneyModifier != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(suggestionOptionXmlModel.moneyModifier, moneySprite);
			optionParameterEntry.gameObject.SetActive(true);
		}

		if (suggestionOptionXmlModel.growthRateModifier != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(suggestionOptionXmlModel.growthRateModifier, growRateSprite);
			optionParameterEntry.gameObject.SetActive(true);
		}

		if (suggestionOptionXmlModel.capacityModifier != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(suggestionOptionXmlModel.capacityModifier, capacitySprite);
			optionParameterEntry.gameObject.SetActive(true);
		}

		if (suggestionOptionXmlModel.vaccineModifier != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(suggestionOptionXmlModel.vaccineModifier, vaccineSprite);
			optionParameterEntry.gameObject.SetActive(true);
		}
	}

	private OptionParameterEntry CreateOptionParameterEntry()
	{
		GameObject optionParameterEntry = GameObjectFactory.instance.InstantiateGameObject(OptionParameterEntry.PREFAB, parametersGridLayoutGroup.transform, false);
		optionParameterEntry.gameObject.transform.SetParent(parametersGridLayoutGroup.transform, true);
		OptionParameterEntry optionParameterEntryScript = optionParameterEntry.GetComponent<OptionParameterEntry>();
		optionParameterEntryScript.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return optionParameterEntryScript;
	}

	public void OnOkButtonSelected()
	{
		suggestionResultEntryChef.Cook(suggestionResultEntryChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void OnExitRecipeCompleted()
	{
		Debug.Log("Suggestion Result Exit Recipe completed");

		GameObjectUtils.instance.DestroyAllChildren(parametersGridLayoutGroup.transform);

		SendExitCompletedEvent();
	}

	private void SendExitCompletedEvent()
	{
		SuggestionResultEntryExitCompletedEvent exitCompletedEvent = SuggestionResultEntryExitCompletedEvent.CreateInstance(this, suggestionOptionXmlModel);
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

using UnityEngine;
using Messages;
using UnityEngine.UI;

public class NextDayEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/NextDayEntry";

	public NextDayEntryChef nextDayEntryChef;

	public GridLayoutGroup parametersGridLayoutGroup;
	public Sprite moneySprite;
	public Sprite growRateSprite;
	public Sprite vaccineSprite;
	public Text dayValue;

	public void SetParameters(GameSessionXmlModel gameSessionXmlModel, int day)
	{
		if (gameSessionXmlModel.nextDayMoneyIncrement != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(gameSessionXmlModel.nextDayMoneyIncrement, moneySprite);
			optionParameterEntry.gameObject.SetActive(true);
		}

		if (gameSessionXmlModel.nextDayGrowthRateIncrement != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(gameSessionXmlModel.nextDayGrowthRateIncrement, growRateSprite);
			optionParameterEntry.gameObject.SetActive(true);
		}

		if (gameSessionXmlModel.nextDayVaccineIncrement != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(gameSessionXmlModel.nextDayVaccineIncrement, vaccineSprite);
			optionParameterEntry.gameObject.SetActive(true);
		}

		dayValue.text = day.ToString();
	}

	public void OnButtonSelected()
	{
		nextDayEntryChef.Cook(nextDayEntryChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void OnExitRecipeCompleted()
	{
		GameObjectUtils.instance.DestroyAllChildren(parametersGridLayoutGroup.transform);

		SendExitCompletedEvent();

		gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(gameObject, PREFAB);
	}

	private void SendExitCompletedEvent()
	{
		NextDayEntryExitCompletedEvent exitCompletedEvent = NextDayEntryExitCompletedEvent.CreateInstance();
		EventMessage exitCompletedEventMessage = new EventMessage(this, exitCompletedEvent);
		exitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(exitCompletedEventMessage);
	}

	private void SendEnterCompletedEvent()
	{
		NextDayEntryEnterCompletedEvent enterCompletedEvent = NextDayEntryEnterCompletedEvent.CreateInstance();
		EventMessage enterCompletedEventMessage = new EventMessage(this, enterCompletedEvent);
		enterCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(enterCompletedEventMessage);
	}

	public void PlayEnterRecipe()
	{
		nextDayEntryChef.Cook(nextDayEntryChef.onEnterRecipe, OnEnterRecipeCompleted);
	}

	private void OnEnterRecipeCompleted()
	{
		SendEnterCompletedEvent();
	}

	private OptionParameterEntry CreateOptionParameterEntry()
	{
		GameObject optionParameterEntry = GameObjectFactory.instance.InstantiateGameObject(OptionParameterEntry.PREFAB, parametersGridLayoutGroup.transform, false);
		optionParameterEntry.gameObject.transform.SetParent(parametersGridLayoutGroup.transform, true);
		OptionParameterEntry optionParameterEntryScript = optionParameterEntry.GetComponent<OptionParameterEntry>();
		optionParameterEntryScript.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return optionParameterEntryScript;
	}
}

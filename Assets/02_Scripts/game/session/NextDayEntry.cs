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
	public Sprite patientsSprite;
	public Text dayValue;
	public Text nextDayValue;

	public void SetParameters(GameSessionXmlModel gameSessionXmlModel, int day)
	{
		ShowVaccineIncrement(gameSessionXmlModel);

		ShowPatientsIncrement(day);

		ShowMoneyIncrement(gameSessionXmlModel);

		ShowGrowthRateIncrement(gameSessionXmlModel);

		dayValue.text = day.ToString();

		nextDayValue.text = (day + 1).ToString();
	}

	private void ShowVaccineIncrement(GameSessionXmlModel gameSessionXmlModel)
	{
		if (gameSessionXmlModel.nextDayVaccineIncrement != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(gameSessionXmlModel.nextDayVaccineIncrement, vaccineSprite);
			optionParameterEntry.gameObject.SetActive(true);
		}
	}

	private void ShowPatientsIncrement(int day)
	{
		GameSession gameSession = GameManager.instance.localPlayer.gameSession;
		int patientsIncrement;
		
		if (day == 0)
		{
			patientsIncrement = gameSession.gameSessionXmlModel.initialPatients - gameSession.GetPreviousDayPatientsForTutorialDayZero();
		}
		else
		{
			patientsIncrement = gameSession.patients[day] - gameSession.patients[day - 1];
		}


		OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
		optionParameterEntry.SetParameter(patientsIncrement, patientsSprite);
		optionParameterEntry.gameObject.SetActive(true);
	}

	private void ShowMoneyIncrement(GameSessionXmlModel gameSessionXmlModel)
	{
		if (gameSessionXmlModel.nextDayMoneyIncrement != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(gameSessionXmlModel.nextDayMoneyIncrement, moneySprite);
			optionParameterEntry.gameObject.SetActive(true);
		}
	}

	private void ShowGrowthRateIncrement(GameSessionXmlModel gameSessionXmlModel)
	{
		if (gameSessionXmlModel.nextDayGrowthRateIncrement != 0)
		{
			OptionParameterEntry optionParameterEntry = CreateOptionParameterEntry();
			optionParameterEntry.SetParameter(gameSessionXmlModel.nextDayGrowthRateIncrement, growRateSprite);
			optionParameterEntry.gameObject.SetActive(true);
		}
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

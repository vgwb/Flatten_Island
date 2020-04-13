using UnityEngine;
using Messages;

public class NextDayEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/NextDayEntry";

	public NextDayEntryChef nextDayEntryChef;

	public void OnButtonSelected()
	{
		nextDayEntryChef.Cook(nextDayEntryChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void OnExitRecipeCompleted()
	{
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
}

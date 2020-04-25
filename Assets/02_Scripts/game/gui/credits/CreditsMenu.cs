using UnityEngine;
using Messages;
using System.Collections;

public class CreditsMenu : MonoBehaviour
{
	public CreditsMenuChef creditsMenuChef;
	public GameObject scrollingPanel;
	private Vector3? scrollingPanelInitialPosition;

	public void PlayCredits()
	{
		if (!scrollingPanelInitialPosition.HasValue)
		{
			scrollingPanelInitialPosition = scrollingPanel.transform.position;
		}
		else
		{
			scrollingPanel.transform.position = scrollingPanelInitialPosition.Value;
		}

		creditsMenuChef.Cook(creditsMenuChef.playCreditsRecipe, OnPlayCreditsCompleted);
	}

	public void OnExitButtonClick()
	{
		SendPlayCreditsCompletedEvent();
	}

	private void OnPlayCreditsCompleted()
	{
		SendPlayCreditsCompletedEvent();
	}

	private void SendPlayCreditsCompletedEvent()
	{
		PlayCreditsCompletedEvent playCreditsCompletedEvent = PlayCreditsCompletedEvent.CreateInstance(this);
		EventMessage playCreditsCompletedEventMessage = new EventMessage(this, playCreditsCompletedEvent);
		playCreditsCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(playCreditsCompletedEventMessage);
	}
}

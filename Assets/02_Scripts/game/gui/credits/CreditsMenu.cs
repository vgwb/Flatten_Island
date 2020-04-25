using UnityEngine;
using Messages;
using System.Collections;

public class CreditsMenu : MonoBehaviour
{
	public CreditsMenuChef creditsMenuChef;
	public GameObject fadeImage;

	public void PlayCredits()
	{
		creditsMenuChef.Cook(creditsMenuChef.playCreditsRecipe, OnPlayCreditsCompleted);
	}

	private void OnPlayCreditsCompleted()
	{
		PlayCreditsCompletedEvent playCreditsCompletedEvent = PlayCreditsCompletedEvent.CreateInstance(this);
		EventMessage playCreditsCompletedEventMessage = new EventMessage(this, playCreditsCompletedEvent);
		playCreditsCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(playCreditsCompletedEventMessage);
	}

	private void OnDisable()
	{
	}
}

using UnityEngine;
using Messages;
using System.Collections;

public class CinematicMenu : MonoBehaviour
{
	public CinematicMenuChef cinematicMenuChef;
	public GameObject cinematicScenesContainer;
	public GameObject fadeImage;

	public void PlayCinematic()
	{
		cinematicMenuChef.Cook(cinematicMenuChef.playCinematicRecipe, OnPlayCinematicCompleted);
	}

	private void OnPlayCinematicCompleted()
	{
		PlayCinematicCompletedEvent playCinematicCompletedEvent = PlayCinematicCompletedEvent.CreateInstance(this);
		EventMessage playCinematicCompletedEventMessage = new EventMessage(this, playCinematicCompletedEvent);
		playCinematicCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(playCinematicCompletedEventMessage);
	}

	private void OnDisable()
	{
		GameObjectUtils.instance.DestroyAllChildren(cinematicScenesContainer.transform);
	}
}

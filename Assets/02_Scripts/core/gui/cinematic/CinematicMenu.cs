using UnityEngine;
using Messages;
using System.Collections;

public class CinematicMenu : MonoBehaviour
{
	public CinematicMenuChef cinematicMenuChef;
	public GameObject cinematicScenesContainer;
	public GameObject fadeImage;

	private GameObject currentScene = null;

	public void PlayCinematic()
	{
		cinematicMenuChef.Cook(cinematicMenuChef.playCinematicRecipe, OnPlayCinematicCompleted);
	}

	public void ShowScene(GameObject cinematicScenePrefab)
	{
		if (currentScene != null)
		{
			currentScene.SetActive(false);
		}

		currentScene = Instantiate(cinematicScenePrefab, cinematicScenesContainer.transform);
	}

	public GameObject GetCurrentScene()
	{
		return currentScene;
	}

	public bool HasCurrentScene()
	{
		return currentScene != null;
	}

	public void HideCurrentScene()
	{
		if (HasCurrentScene())
		{
			currentScene.SetActive(false);
			currentScene = null;
		}
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

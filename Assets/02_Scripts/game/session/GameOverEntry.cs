using UnityEngine;
using Messages;
using UnityEngine.UI;

public class GameOverEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/GameOverEntry";

	public GameOverEntryChef gameOverEntryChef;

	public Text titleText;
	public Text messageText;
	public Text okButtonText;

	public GameOverXmlModel gameOverXmlModel;

	public void SetParameters(bool hasPlayerWon)
	{
		if (gameOverXmlModel == null)
		{
			gameOverXmlModel = XmlModelManager.instance.FindModel<GameOverXmlModel>();
		}

		if (hasPlayerWon)
		{
			LocalizationManager.instance.SetLocalizedText(titleText, gameOverXmlModel.winTitle);
			LocalizationManager.instance.SetLocalizedText(messageText, gameOverXmlModel.winDescription);
			LocalizationManager.instance.SetLocalizedText(okButtonText, gameOverXmlModel.winButton);
		}
		else
		{
			LocalizationManager.instance.SetLocalizedText(titleText, gameOverXmlModel.loseTitle);
			LocalizationManager.instance.SetLocalizedText(messageText, gameOverXmlModel.loseDescription);
			LocalizationManager.instance.SetLocalizedText(okButtonText, gameOverXmlModel.loseButton);
		}
	}

	public void OnButtonSelected()
	{
		gameOverEntryChef.Cook(gameOverEntryChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void OnExitRecipeCompleted()
	{
		SendExitCompletedEvent();

		gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(gameObject, PREFAB);
	}

	private void SendExitCompletedEvent()
	{
		GameOverEntryExitCompletedEvent exitCompletedEvent = GameOverEntryExitCompletedEvent.CreateInstance();
		EventMessage exitCompletedEventMessage = new EventMessage(this, exitCompletedEvent);
		exitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(exitCompletedEventMessage);
	}

	private void SendEnterCompletedEvent()
	{
		GameOverEntryEnterCompletedEvent enterCompletedEvent = GameOverEntryEnterCompletedEvent.CreateInstance();
		EventMessage enterCompletedEventMessage = new EventMessage(this, enterCompletedEvent);
		enterCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(enterCompletedEventMessage);
	}

	public void PlayEnterRecipe()
	{
		gameOverEntryChef.Cook(gameOverEntryChef.onEnterRecipe, OnEnterRecipeCompleted);
	}

	private void OnEnterRecipeCompleted()
	{
		SendEnterCompletedEvent();
	}
}

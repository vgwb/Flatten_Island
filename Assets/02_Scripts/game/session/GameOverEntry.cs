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
	public Text daysText;
	public string daysLocalizationId;
	public GameObject winTitleGroup;
	public GameObject loseTitleGroup;
	public GameObject newHighScoreGroup;
	public AudioClip victoryAudioClip;
	public AudioClip gameOverAudioClip;

	private Canvas titleCanvas;
	private Canvas messageCanvas;

	public GameOverXmlModel gameOverXmlModel;
	private bool hasPlayerWon;

	private void Awake()
	{
		titleCanvas = titleText.GetComponent<Canvas>();
		messageCanvas = messageText.GetComponent<Canvas>();
	}

	public void SetParameters(bool hasPlayerWon)
	{
		if (gameOverXmlModel == null)
		{
			gameOverXmlModel = XmlModelManager.instance.FindModel<GameOverXmlModel>();
		}

		winTitleGroup.SetActive(hasPlayerWon);
		loseTitleGroup.SetActive(!hasPlayerWon);
		daysText.gameObject.SetActive(hasPlayerWon);
		this.hasPlayerWon = hasPlayerWon;

		if (hasPlayerWon)
		{
			LocalizationManager.instance.SetLocalizedText(titleText, gameOverXmlModel.winTitle);
			LocalizationManager.instance.SetLocalizedText(messageText, gameOverXmlModel.winDescription);
			LocalizationManager.instance.SetLocalizedText(okButtonText, gameOverXmlModel.winButton);
			newHighScoreGroup.SetActive(GameManager.instance.HasNewDayHighScore());
			LocalizationManager.instance.SetLocalizedText(daysText, daysLocalizationId);
			daysText.text = GameManager.instance.localPlayer.gameSession.day + " " + daysText.text;
		}
		else
		{
			LocalizationManager.instance.SetLocalizedText(titleText, gameOverXmlModel.loseTitle);
			LocalizationManager.instance.SetLocalizedText(messageText, gameOverXmlModel.loseDescription);
			LocalizationManager.instance.SetLocalizedText(okButtonText, gameOverXmlModel.loseButton);
		}
	}

	public void RefreshCanvas()
	{
		gameObject.SetActive(true);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

		//I don't know if there is a bug in Unity, maybe it's related to setting the localScale,
		//but text message were not displayed. I had to force the canvas to refresh
		titleCanvas.sortingOrder = titleCanvas.sortingOrder;
		messageCanvas.sortingOrder = messageCanvas.sortingOrder;
	}

	public void OnButtonSelected()
	{
		gameOverEntryChef.Cook(gameOverEntryChef.onExitRecipe, OnExitRecipeCompleted);
		AudioManager.instance.StopOneShot();
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

	public void PlayAudioClip()
	{
		if (hasPlayerWon)
		{
			AudioManager.instance.PlayOneShot(victoryAudioClip, EAudioChannelType.Sfx);
		}
		else
		{
			AudioManager.instance.PlayOneShot(gameOverAudioClip, EAudioChannelType.Sfx);
		}
	}

	private void OnEnterRecipeCompleted()
	{
		SendEnterCompletedEvent();
	}
}

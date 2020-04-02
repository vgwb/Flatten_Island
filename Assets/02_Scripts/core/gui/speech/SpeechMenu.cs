using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Messages;

public class SpeechMenu : MonoBehaviour
{
	public SpeechBubble speechBubble;
	public Button blockingBackgrounButton;

	public void OnBackgroundButtonTapped()
	{
		BackgroundButtonTappedEvent backgroundButtonTappedEvent = BackgroundButtonTappedEvent.CreateInstance(this);
		EventMessage backgroundButtonTappedEventMessage = new EventMessage(this, backgroundButtonTappedEvent);
		backgroundButtonTappedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(backgroundButtonTappedEventMessage);
	}

	public void ShowSpeechBubble(string localizationId)
	{
		string text = LocalizationManager.instance.GetText(localizationId);

		speechBubble.SetText(text);
		speechBubble.gameObject.SetActive(true);
		blockingBackgrounButton.gameObject.SetActive(true);
	}

	public void HideSpeechBubble()
	{
		speechBubble.gameObject.SetActive(false);
		blockingBackgrounButton.gameObject.SetActive(false);
	}
}

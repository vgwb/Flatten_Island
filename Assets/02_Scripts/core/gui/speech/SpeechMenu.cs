using UnityEngine;
using System.Collections;

public class SpeechMenu : MonoBehaviour
{
	public SpeechBubble speechBubble;

	public void TestOnClick()
	{
		Debug.Log("On Speech menu click");
	}

	public void ShowSpeechBubble(string localizetionTextKey)
	{
		//ToDo: localize string
		string text = localizetionTextKey;

		speechBubble.SetText(text);
		speechBubble.gameObject.SetActive(true);
	}

	public void HideSpeechBubble()
	{
		speechBubble.gameObject.SetActive(false);
	}
}

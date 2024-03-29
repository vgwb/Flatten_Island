﻿using UnityEngine;
using UnityEngine.UI;

public class SpeechBubble : MonoBehaviour
{
	public Text speechText;
	public Image speechBubble;

	public void SetText(string text)
	{
		speechText.text = text;
	}

	public void SetLocalizedText(string localizationId)
	{
		LocalizationManager.instance.SetLocalizedText(speechText, localizationId);
	}

}

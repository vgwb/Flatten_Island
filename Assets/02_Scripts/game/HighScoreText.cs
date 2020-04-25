using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{
    public Text highScoreText;
	public string localizationId;

	public void DisplayDayHighScore(int day)
	{
		LocalizationManager.instance.SetLocalizedText(highScoreText, localizationId);
		highScoreText.text = LocalizationManager.instance.ReplaceIntInText(highScoreText.text, day);
	}
}

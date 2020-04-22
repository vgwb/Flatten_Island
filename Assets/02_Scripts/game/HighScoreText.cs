using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreText : MonoBehaviour
{

    public Text highScoreText;

	public void DisplayHighScoreText(){

		highScoreText.text = GameManager.instance.localPlayer.highScore.day.ToString();			

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreMenuText : MonoBehaviour
{

    public Text highScoreText;
	
	void Start()
	{
         DisplayHighScoreText();
	}

	public void DisplayHighScoreText(){

		highScoreText.text = GameManager.instance.localPlayer.highScore.day.ToString( );			

	}
	
}

using System;
using UnityEngine;

public class WatchIntroButton : MonoBehaviour
{
	public void OnClick()
	{
		MenuScene.instance.WatchCinematic();
	}
}
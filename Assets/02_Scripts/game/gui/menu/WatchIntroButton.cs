using System;
using UnityEngine;

public class WatchIntroButton : MonoBehaviour
{
	public void OnClick()
	{
		SettingsMenu.instance.HideMenu();
		MenuScene.instance.WatchCinematic();
	}
}
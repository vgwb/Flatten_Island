using System;
using UnityEngine;

public class PrivacyButton : MonoBehaviour
{
	public void OnClick()
	{
		SettingsMenu.instance.HideMenu();
		PrivacyDialog.Show(MenuScene.instance.menuCanvas.transform);
	}
}
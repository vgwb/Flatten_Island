using System;
using UnityEngine;

public class PrivacyButton : MonoBehaviour
{
	public void OnClick()
	{
		SettingsMenu.instance.HideMenu();
	}
}
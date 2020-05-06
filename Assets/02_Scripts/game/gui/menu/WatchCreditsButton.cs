using UnityEngine;

public class WatchCreditsButton : MonoBehaviour
{
	public void OnClick()
	{
		SettingsMenu.instance.HideMenu();
		MenuScene.instance.WatchCredits();
	}
}
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoSingleton
{
	public GridLayoutGroup settingsGrid;
	public Button settingsButton;

	public static SettingsMenu instance
	{
		get
		{
			return GetInstance<SettingsMenu>();
		}
	}

	public void HideMenu()
	{
		settingsGrid.gameObject.SetActive(false);
		LanguageMenu.instance.ShowLanguagesButton(true);
		MenuScene.instance.UpdateBlockingBackground();
	}

	public void ShowSettingsButton(bool shown)
	{
		settingsButton.gameObject.SetActive(shown);
	}

	public void OnButtonShowMenuClick()
	{
		ShowGrid(!settingsGrid.gameObject.activeInHierarchy);
		LanguageMenu.instance.ShowLanguagesButton(!settingsGrid.gameObject.activeInHierarchy);
	}

	private void ShowGrid(bool visible)
	{
		settingsGrid.gameObject.SetActive(visible);
		MenuScene.instance.UpdateBlockingBackground();
	}

	public bool IsGridShown()
	{
		return settingsGrid.gameObject.activeInHierarchy;
	}
}

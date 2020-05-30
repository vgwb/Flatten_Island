using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class LanguageMenu : MonoSingleton
{
	public GridLayoutGroup languagesGrid;
	public Button languagesButton;

	public static LanguageMenu instance
	{
		get
		{
			return GetInstance<LanguageMenu>();
		}
	}

	public bool IsGridShown()
	{
		return languagesGrid.gameObject.activeInHierarchy;
	}

	public void HideMenu()
	{
		languagesGrid.gameObject.SetActive(false);
		SettingsMenu.instance.ShowSettingsButton(true);
		MenuScene.instance.UpdateBlockingBackground();
	}

	public void ShowLanguagesButton(bool shown)
	{
		languagesButton.gameObject.SetActive(shown);
	}

	public void OnButtonShowMenuClick()
	{
		ShowGrid(!languagesGrid.gameObject.activeInHierarchy);
		SettingsMenu.instance.ShowSettingsButton(!languagesGrid.gameObject.activeInHierarchy);
	}

	private void ShowGrid(bool visible)
	{
		languagesGrid.gameObject.SetActive(visible);
		MenuScene.instance.UpdateBlockingBackground();
	}

}
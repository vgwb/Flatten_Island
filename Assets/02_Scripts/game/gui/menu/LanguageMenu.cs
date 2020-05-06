using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class LanguageMenu : MonoSingleton
{
	public GridLayoutGroup languagesGrid;

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
		MenuScene.instance.UpdateBlockingBackground();
	}

	public void OnButtonShowMenuClick()
	{
		ShowGrid(!languagesGrid.gameObject.activeInHierarchy);
	}

	private void ShowGrid(bool visible)
	{
		languagesGrid.gameObject.SetActive(visible);
		MenuScene.instance.UpdateBlockingBackground();
	}

}
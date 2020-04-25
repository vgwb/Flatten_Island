using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class LanguageMenu : MonoBehaviour
{
	public GridLayoutGroup languagesGrid;

	public bool IsGridShown()
	{
		return languagesGrid.gameObject.activeInHierarchy;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class SettingsMenu : MonoBehaviour
{
	public GridLayoutGroup settingsGrid;

	[SerializeField]
	private GameObject CreditsPanel;


	public void OnButtonShowMenuClick()
	{
		ShowGrid(!settingsGrid.gameObject.activeInHierarchy);
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

	public void OpenShowCreditsPanel()
	{
		CreditsPanel.SetActive (true);

	}

	public void CloseShowCreditsPanel()
	{
		CreditsPanel.SetActive (false);

	}
}

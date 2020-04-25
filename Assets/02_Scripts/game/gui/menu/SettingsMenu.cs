using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	public GridLayoutGroup settingsGrid;

	[SerializeField]
	private GameObject CreditsPanel;
	[SerializeField]
	private GameObject WatchIntroPanel;
	[SerializeField]
	private GameObject ShowPanelSettings;
    public static bool isPanelShow;
    private bool isPanelHide = true;


	public void OnButtonShowMenuClick()
	{
		ShowGrid(!settingsGrid.gameObject.activeInHierarchy);
	}

	private void ShowGrid(bool visible)
	{
		settingsGrid.gameObject.SetActive(visible);
	}


	public void ShowPanelSettingsMenu() {

        if (Time.timeScale == 1)
        {
            ShowPanelSettings.gameObject.SetActive(true);
            Time.timeScale = 0;
            isPanelShow = true;
        }else if (Time.timeScale == 0)
        {
            ShowPanelSettings.gameObject.SetActive(false);
            Time.timeScale = 1;
            isPanelShow = false;
        }
	}
	public void OpenShowCreditsPanel()
	{
		ShowPanelSettings.SetActive (false);
		CreditsPanel.SetActive (true);

	}
    public void CloseShowCreditsPanel()
	{
		ShowPanelSettings.SetActive (true);
		CreditsPanel.SetActive (false);

	} 
	public void OpenWatchIntroPanel()
	{
		ShowPanelSettings.SetActive(false);
		WatchIntroPanel.SetActive(true);
	}
	public void CloseWatchIntroPanel()
	{
		ShowPanelSettings.SetActive (true);
		WatchIntroPanel.SetActive (false);
	}
}

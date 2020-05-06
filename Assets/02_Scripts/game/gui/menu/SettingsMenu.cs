using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoSingleton
{
	public GridLayoutGroup settingsGrid;

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
		MenuScene.instance.UpdateBlockingBackground();
	}

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
}

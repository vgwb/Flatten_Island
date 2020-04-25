using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
	public GridLayoutGroup settingsGrid;

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

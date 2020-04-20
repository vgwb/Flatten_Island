using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CheatMenu : MonoBehaviour
{
	public static string PREFAB = "GUI/CheatsMenu";
	public Button showSuggestionParametersButton;

	public void Close()
	{
		gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(gameObject, PREFAB);
	}

	public void ToggleSuggestionParamaters()
	{
		CheatManager.instance.ToggleSuggestionParameters();
		ChangeToggleButtonStatus(showSuggestionParametersButton, CheatManager.instance.showSuggestionParameters);
	}

	private static CheatMenu CreateCheatMenu()
	{
		Transform parent = MainScene.instance.uiWorldCanvas.transform;
		GameObject cheatMenuObject = GameObjectFactory.instance.InstantiateGameObject(PREFAB, parent, false);
		cheatMenuObject.transform.SetParent(parent, true);
		CheatMenu cheatMenu = cheatMenuObject.GetComponent<CheatMenu>();
		return cheatMenu;
	}

	public static CheatMenu Show()
	{
		CheatMenu cheatMenu = CreateCheatMenu();
		cheatMenu.gameObject.SetActive(true);
		cheatMenu.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

		cheatMenu.ChangeToggleButtonStatus(cheatMenu.showSuggestionParametersButton, CheatManager.instance.showSuggestionParameters);

		return cheatMenu;
	}

	private void ChangeToggleButtonStatus(Button button, bool cheatEnabled)
	{
		Image buttonImage = button.gameObject.GetComponent<Image>();

		ColorBlock buttonColors = button.colors;
		if (cheatEnabled)
		{
			buttonColors.normalColor = Color.green;
			buttonColors.pressedColor = Color.green;

			if (buttonImage != null)
			{
				buttonImage.color = Color.green;
			}
		}
		else
		{
			buttonColors.normalColor = Color.red;
			buttonColors.pressedColor = Color.red;

			if (buttonImage != null)
			{
				buttonImage.color = Color.red;
			}
		}

		button.colors = buttonColors;
	}
}
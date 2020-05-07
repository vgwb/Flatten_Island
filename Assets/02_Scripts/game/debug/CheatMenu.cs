using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class CheatMenu : MonoBehaviour
{
	public static string PREFAB = "GUI/CheatsMenu";
	public Button showSuggestionParametersButton;
	public GameObject suggestionsPanel;
	public GameObject layout;

	private List<SuggestionXmlModel> suggestionXmlModels;
	private SuggestionMenu suggestionMenu;
	private AdvisorsMenu advisorMenu;
	private SuggestionEntry suggestionEntry;
	private int suggestionEntryIndex;

	public void Close()
	{
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionEntryExitCompletedEvent).Name, this);

		suggestionsPanel.SetActive(false);
		suggestionMenu.gameObject.SetActive(true);
		advisorMenu.gameObject.SetActive(true);

		if (suggestionEntry != null)
		{
			suggestionEntry.gameObject.SetActive(false);
			GameObjectFactory.instance.ReleaseGameObject(suggestionEntry.gameObject, SuggestionEntry.PREFAB);
			suggestionEntry = null;
		}

		gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(gameObject, PREFAB);
	}

	public void ToggleSuggestionParamaters()
	{
		CheatManager.instance.ToggleSuggestionParameters();
		ChangeToggleButtonStatus(showSuggestionParametersButton, CheatManager.instance.showSuggestionParameters);
	}

	public void ShowSuggestionsText()
	{
		if (suggestionEntry == null)
		{
			suggestionsPanel.SetActive(true);
			layout.SetActive(false);
			suggestionMenu = AdvisorsManager.instance.suggestionMenu;
			suggestionMenu.gameObject.SetActive(false);
			advisorMenu = AdvisorsManager.instance.advisorMenu;
			advisorMenu.gameObject.SetActive(false);

			EventMessageHandler suggestionEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionEntryExitCompleted);
			EventMessageManager.instance.AddHandler(typeof(SuggestionEntryExitCompletedEvent).Name, suggestionEntryExitCompletedMessageHandler);

			if (suggestionXmlModels == null)
			{
				suggestionXmlModels = XmlModelManager.instance.FindModels<SuggestionXmlModel>();
			}

			ShowNextSuggestionEntry();
		}
	}

	private void ShowNextSuggestionEntry()
	{
		suggestionEntryIndex = (suggestionEntryIndex + 1) % suggestionXmlModels.Count;

		SuggestionXmlModel suggestionXmlModel = suggestionXmlModels[suggestionEntryIndex];

		AdvisorXmlModel advisorXmlModel = XmlModelManager.instance.FindModel<AdvisorXmlModel>(suggestionXmlModel.advisorId);

		ShowSuggestion(suggestionXmlModel, advisorXmlModel);
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

	public void ShowSuggestion(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		suggestionEntry = CreateSuggestionEntry(suggestionXmlModel, advisorXmlModel);
		suggestionEntry.PlayEnterRecipe();
	}

	private SuggestionEntry CreateSuggestionEntry(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		GameObject suggestionEntry = GameObjectFactory.instance.InstantiateGameObject(SuggestionEntry.PREFAB, suggestionsPanel.transform, false);
		suggestionEntry.gameObject.transform.SetParent(suggestionsPanel.transform, true);
		SuggestionEntry suggestionEntryScript = suggestionEntry.GetComponent<SuggestionEntry>();
		suggestionEntryScript.SetSuggestion(suggestionXmlModel, advisorXmlModel);
		suggestionEntry.gameObject.SetActive(true);
		suggestionEntry.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return suggestionEntryScript;
	}

	private void OnSuggestionEntryExitCompleted(EventMessage eventMessage)
	{
		suggestionEntry.gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(suggestionEntry.gameObject, SuggestionEntry.PREFAB);

		ShowNextSuggestionEntry();
	}
}
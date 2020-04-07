using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class SuggestionEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/SuggestionEntry";

	public Image advisorIcon;
	public Text title;
	public Text description;
	public SuggestionButton buttonOptionA;
	public SuggestionButton buttonOptionB;

	public SuggestionXmlModel suggestionXmlModel;

	public void SetSuggestion(SuggestionXmlModel suggestionXmlModel)
	{
		this.suggestionXmlModel = suggestionXmlModel;

		title.text = LocalizationManager.instance.GetText(suggestionXmlModel.name);
	}

	public void OnButtonOptionASelected()
	{

	}

	public void OnButtonOptionBSelected()
	{

	}
}

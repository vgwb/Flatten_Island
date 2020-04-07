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

	public void SetSuggestion(SuggestionXmlModel suggestionXmlModel, AdvisorXmlModel advisorXmlModel)
	{
		this.suggestionXmlModel = suggestionXmlModel;

		title.text = LocalizationManager.instance.GetText(suggestionXmlModel.title);
		description.text = LocalizationManager.instance.GetText(suggestionXmlModel.description);

		if (!string.IsNullOrEmpty(advisorXmlModel.iconSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.iconSprite);
			advisorIcon.overrideSprite = advisorSprite;
		}

		SuggestionOptionXmlModel optionAXmlModel = suggestionXmlModel.suggestionOptionsList[0];
		SuggestionOptionXmlModel optionBXmlModel = suggestionXmlModel.suggestionOptionsList[1];

		buttonOptionA.SetButton(optionAXmlModel);
		buttonOptionB.SetButton(optionBXmlModel);
	}

	public void OnButtonOptionASelected()
	{
		Debug.Log("Suggestion - Option A Selected");
	}

	public void OnButtonOptionBSelected()
	{
		Debug.Log("Suggestion - Option B Selected");
	}
}

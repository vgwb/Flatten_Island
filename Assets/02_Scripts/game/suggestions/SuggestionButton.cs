using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SuggestionButton : MonoBehaviour
{
	public Text buttonText;
	public Text buttonCostText;
	public Image buttonCostIcon;

	public void SetButton(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		buttonText.text = LocalizationManager.instance.GetText(suggestionOptionXmlModel.text);
		if (suggestionOptionXmlModel.moneyModifier == 0)
		{
			buttonCostIcon.gameObject.SetActive(false);
			buttonCostText.text = LocalizationManager.instance.GetText("FREE");
		}
		else
		{
			buttonCostIcon.gameObject.SetActive(true);
			buttonCostText.text = suggestionOptionXmlModel.moneyModifier.ToString();
		}
	}
}
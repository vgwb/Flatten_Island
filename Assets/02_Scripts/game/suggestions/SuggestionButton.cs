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

	public void SetButton(string buttonTextId, string buttonCostTextid, bool showCostIcon)
	{
		buttonText.text = LocalizationManager.instance.GetText(buttonTextId);
		buttonCostText.text = LocalizationManager.instance.GetText(buttonCostTextid);
		buttonCostIcon.gameObject.SetActive(showCostIcon);
	}
}
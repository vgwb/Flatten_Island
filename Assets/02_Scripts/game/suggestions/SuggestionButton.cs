using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SuggestionButton : MonoBehaviour
{
	public Text buttonText;
	public Text buttonCostText;
	public Image buttonCostIcon;
	public Text debugText;

	public void SetButton(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		LocalizationManager.instance.SetLocalizedText(buttonText, suggestionOptionXmlModel.text);
		if (suggestionOptionXmlModel.moneyModifier == 0)
		{
			//buttonCostIcon.gameObject.SetActive(false);
			LocalizationManager.instance.SetLocalizedText(buttonCostText, "FREE");
		}
		else
		{
			//buttonCostIcon.gameObject.SetActive(true); //disabling for now
			buttonCostText.text = suggestionOptionXmlModel.moneyModifier.ToString();
		}

		TrySetDebugText(suggestionOptionXmlModel);
	}

	private void TrySetDebugText(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
#if CHEAT_DEBUG
		if (CheatManager.instance.showSuggestionParameters)
		{
			StringBuilder debugString = new StringBuilder();
			if (suggestionOptionXmlModel.moneyModifier != 0)
			{
				debugString.Append("$ " + suggestionOptionXmlModel.moneyModifier);
			}

			if (suggestionOptionXmlModel.growthRateModifier != 0)
			{
				if (debugString.Length > 0)
				{
					debugString.Append(" / ");
				}

				debugString.Append("G " + suggestionOptionXmlModel.growthRateModifier);
			}

			if (suggestionOptionXmlModel.capacityModifier != 0)
			{
				if (debugString.Length > 0)
				{
					debugString.Append(" / ");
				}

				debugString.Append("C " + suggestionOptionXmlModel.capacityModifier);
			}

			if (suggestionOptionXmlModel.publicOpinionModifier != 0)
			{
				if (debugString.Length > 0)
				{
					debugString.Append(" / ");
				}

				debugString.Append("P " + suggestionOptionXmlModel.publicOpinionModifier);
			}

			if (suggestionOptionXmlModel.vaccineModifier != 0)
			{
				if (debugString.Length > 0)
				{
					debugString.Append(" / ");
				}

				debugString.Append("V " + suggestionOptionXmlModel.vaccineModifier);
			}

			debugText.text = debugString.ToString();
			debugText.gameObject.SetActive(true);
		}
		else
		{
			debugText.gameObject.SetActive(false);
		}
#else
		debugText.gameObject.SetActive(false);
#endif
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class OptionParameterEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/OptionParameterEntry";

	public Text modifierValueText;
	public Image modifierIcon;
	public Color positiveValueColor;
	public Color negativeValueColor;

	public void SetParameter(int modifierValue, Sprite sprite)
	{
		modifierValueText.text = modifierValue.ToString();
		modifierIcon.sprite = sprite;

		if (modifierValue > 0)
		{
			SetTextColor(modifierValueText, positiveValueColor);
		}
		else
		{
			SetTextColor(modifierValueText, negativeValueColor);
		}
	}

	private void SetTextColor(Text text, Color color)
	{
		text.color = color;
	}


}

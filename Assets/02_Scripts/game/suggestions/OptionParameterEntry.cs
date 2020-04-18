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

	public void SetParameter(int modifierValue, Sprite sprite)
	{
		modifierIcon.sprite = sprite;

		if (modifierValue > 0)
		{
			modifierValueText.text = "+" + modifierValue.ToString();
		}
		else
		{
			modifierValueText.text = modifierValue.ToString();
		}
	}
}

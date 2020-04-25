using System;
using UnityEngine;
using UnityEngine.UI;

public class LanguageMenu : MonoBehaviour
{
	public GridLayoutGroup languagesGrid;

	public void OnButtonShowMenuClick()
	{
		languagesGrid.gameObject.SetActive(!languagesGrid.gameObject.activeInHierarchy);
	}
}
using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class LanguageMenu : MonoBehaviour
{
	public GridLayoutGroup languagesGrid;

	private void OnEnable()
	{
		EventMessageHandler languageSelectedMessageHandler = new EventMessageHandler(this, OnLanguageSelectedEvent);
		EventMessageManager.instance.AddHandler(typeof(LanguageSelectedEvent).Name, languageSelectedMessageHandler);
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(LanguageSelectedEvent).Name, this);
	}

	private void OnLanguageSelectedEvent(EventMessage eventMessage)
	{
		ShowGrid(false);
	}

	public bool IsGridShown()
	{
		return languagesGrid.gameObject.activeInHierarchy;
	}

	public void OnButtonShowMenuClick()
	{
		ShowGrid(!languagesGrid.gameObject.activeInHierarchy);
	}

	private void ShowGrid(bool visible)
	{
		languagesGrid.gameObject.SetActive(visible);
		MenuScene.instance.UpdateBlockingBackground();
	}

}
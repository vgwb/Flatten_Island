﻿using UnityEngine;
using UnityEngine.UI;
using Messages;

public class UITextLocalizer : MonoBehaviour
{
	public string localizationId;

	private Text text;

	private void Awake()
	{
		text = GetComponent<Text>();
	}

	private void OnEnable()
	{
		EventMessageHandler languageChangedMessageHandler = new EventMessageHandler(this, OnLanguageChangedEvent);
		EventMessageManager.instance.AddHandler(typeof(LanguageChangedEvent).Name, languageChangedMessageHandler);

		RefreshText();
	}

	private void OnDisable()
	{
		EventMessageManager.instance.RemoveHandler(typeof(LanguageChangedEvent).Name, this);
	}

	private void OnLanguageChangedEvent(EventMessage eventMessage)
	{
		RefreshText();
	}

	private void RefreshText()
	{
		LocalizationManager.instance.SetLocalizedText(text, localizationId);
	}
}
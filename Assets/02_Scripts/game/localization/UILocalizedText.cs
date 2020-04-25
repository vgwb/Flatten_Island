using System;
using UnityEngine;
using Messages;

public class UILocalizedText : MonoBehaviour
{
	private void OnEnable()
	{
		EventMessageHandler languageChangedMessageHandler = new EventMessageHandler(this, OnLanguageChangedEvent);
		EventMessageManager.instance.AddHandler(typeof(LanguageChangedEvent).Name, languageChangedMessageHandler);
		OnEnabled();
	}

	protected virtual void OnEnabled()
	{
	}

	private void OnDisable()
	{
		OnDisabled();
		EventMessageManager.instance.RemoveHandler(typeof(LanguageChangedEvent).Name, this);
	}

	protected virtual void OnDisabled()
	{
	}

	private void OnLanguageChangedEvent(EventMessage eventMessage)
	{
		RefreshText();
	}

	protected virtual void RefreshText()
	{
	}
}
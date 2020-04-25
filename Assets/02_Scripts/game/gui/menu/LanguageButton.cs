using System;
using UnityEngine;
using Messages;

public class LanguageButton : MonoBehaviour
{
	public string languageId;

	private LocalizationXmlModel languageXmlModel;

	private void Awake()
	{
		languageXmlModel = LocalizationManager.instance.FindLocalizationXmlModelWithLanguageId(languageId);
	}

	public void OnClick()
	{
		if (languageXmlModel != null)
		{
			LanguageSelectedEvent languageSelectedEvent = LanguageSelectedEvent.CreateInstance(languageXmlModel);
			EventMessage languageSelectedEventMessage = new EventMessage(this, languageSelectedEvent);
			languageSelectedEventMessage.SetMessageType(MessageType.BROADCAST);
			EventMessageManager.instance.QueueMessage(languageSelectedEventMessage);
		}
	}
}
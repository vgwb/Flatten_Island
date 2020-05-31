using System;
using UnityEngine;
using Messages;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour
{
	public string languageId;
	public Text languageButtonText;

	private LocalizationXmlModel languageXmlModel;

	private void Awake()
	{
		languageXmlModel = LocalizationManager.instance.FindLocalizationXmlModelWithLanguageId(languageId);
		languageButtonText.text = languageXmlModel.name;
	}

	public void OnClick()
	{
		LanguageMenu.instance.HideMenu();

		if (languageXmlModel != null)
		{
			LanguageSelectedEvent languageSelectedEvent = LanguageSelectedEvent.CreateInstance(languageXmlModel);
			EventMessage languageSelectedEventMessage = new EventMessage(this, languageSelectedEvent);
			languageSelectedEventMessage.SetMessageType(MessageType.BROADCAST);
			EventMessageManager.instance.QueueMessage(languageSelectedEventMessage);
		}
	}
}
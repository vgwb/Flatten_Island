using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class AdvisorEntry : MonoBehaviour
{
	public static string PREFAB = "GUI/AdvisorEntry";

	public Image portrait;
	public Text advisorName;
	public AdvisorXmlModel advisorXmlModel;

	public void SetAdvisor(AdvisorXmlModel advisorXmlModel)
	{
		this.advisorXmlModel = advisorXmlModel;

		advisorName.text = LocalizationManager.instance.GetText(advisorXmlModel.name);
		if (!string.IsNullOrEmpty(advisorXmlModel.portraitSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.portraitSprite);
			portrait.overrideSprite = advisorSprite;
		}
	}

	public void OnSelected()
	{
		AdvisorSelectedEvent advisorSelectedEvent = AdvisorSelectedEvent.CreateInstance(this);
		EventMessage advisorSelectedEventMessage = new EventMessage(this, advisorSelectedEvent);
		advisorSelectedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(advisorSelectedEventMessage);
	}
}

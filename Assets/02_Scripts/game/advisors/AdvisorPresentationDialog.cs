using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class AdvisorPresentationDialog : MonoBehaviour
{
	public static string PREFAB = "GUI/AdvisorPresentationDialog";

	public Text presentationText;
	public AdvisorPresentationChef advisorPresentationChef;
	public AdvisorXmlModel advisorXmlModel;

	public void SetDialog(AdvisorXmlModel advisorXmlModel)
	{
		this.advisorXmlModel = advisorXmlModel;
		LocalizationManager.instance.SetLocalizedText(presentationText, advisorXmlModel.presentation);
	}

	public void OnClick()
	{
		advisorPresentationChef.Cook(advisorPresentationChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void OnExitRecipeCompleted()
	{
		Debug.Log("Advisor Presentation " + advisorXmlModel.name + " Exit Recipe completed");
		SendExitCompletedEvent();
	}

	private void SendExitCompletedEvent()
	{
		AdvisorPresentationExitCompletedEvent exitCompletedEvent = AdvisorPresentationExitCompletedEvent.CreateInstance(this);
		EventMessage exitCompletedEventMessage = new EventMessage(this, exitCompletedEvent);
		exitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(exitCompletedEventMessage);
	}

	private void PlayEnterRecipe()
	{
		advisorPresentationChef.Cook(advisorPresentationChef.onEnterRecipe, OnEnterRecipeCompleted);
	}

	private void OnEnterRecipeCompleted()
	{
		Debug.Log("Advisor Presentation" + advisorXmlModel.name + " Enter Recipe completed");
	}

	public static AdvisorPresentationDialog Show(AdvisorXmlModel advisorXmlModel, Transform parent)
	{
		GameObject presentationDialogObject = GameObjectFactory.instance.InstantiateGameObject(PREFAB, parent, false);
		presentationDialogObject.transform.SetParent(parent, true);
		AdvisorPresentationDialog advisorPresentationDialog = presentationDialogObject.GetComponent<AdvisorPresentationDialog>();
		advisorPresentationDialog.SetDialog(advisorXmlModel);
		presentationDialogObject.gameObject.SetActive(true);
		presentationDialogObject.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

		advisorPresentationDialog.PlayEnterRecipe();
		return advisorPresentationDialog;
	}

	public void Release()
	{
		gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(gameObject, PREFAB);
	}
}

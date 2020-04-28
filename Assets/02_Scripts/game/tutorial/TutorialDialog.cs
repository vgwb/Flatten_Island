using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class TutorialDialog : MonoBehaviour
{
	public static string PREFAB = "GUI/TutorialDialog";

	public Text presentationText;
	public TutorialDialogChef tutorialDialogChef;
	public AdvisorXmlModel advisorXmlModel;

	public void SetAdvisorDialog(AdvisorXmlModel advisorXmlModel)
	{
		this.advisorXmlModel = advisorXmlModel;
		LocalizationManager.instance.SetLocalizedText(presentationText, advisorXmlModel.presentation);
	}

	public void SetTutorialTipDialog(string localizationId)
	{
		advisorXmlModel = null;
		LocalizationManager.instance.SetLocalizedText(presentationText, localizationId);
	}

	public void OnClick()
	{
		tutorialDialogChef.Cook(tutorialDialogChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void OnExitRecipeCompleted()
	{
		Debug.Log("Tutorial Dialog: " + advisorXmlModel.name + " Exit Recipe completed");
		SendExitCompletedEvent();
	}

	private void SendExitCompletedEvent()
	{
		TutorialDialogExitCompletedEvent exitCompletedEvent = TutorialDialogExitCompletedEvent.CreateInstance(this);
		EventMessage exitCompletedEventMessage = new EventMessage(this, exitCompletedEvent);
		exitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(exitCompletedEventMessage);
	}

	private void PlayEnterRecipe()
	{
		tutorialDialogChef.Cook(tutorialDialogChef.onEnterRecipe, OnEnterRecipeCompleted);
	}

	private void OnEnterRecipeCompleted()
	{
		Debug.Log("Tutorial Dialog" + advisorXmlModel.name + " Enter Recipe completed");
	}

	public static TutorialDialog ShowAdvisorPresentation(AdvisorXmlModel advisorXmlModel, Transform parent)
	{
		TutorialDialog advisorPresentationDialog = CreateDialog(parent);
		advisorPresentationDialog.SetAdvisorDialog(advisorXmlModel);
		advisorPresentationDialog.PlayEnterRecipe();
		return advisorPresentationDialog;
	}

	private static TutorialDialog CreateDialog(Transform parent)
	{
		GameObject presentationDialogObject = GameObjectFactory.instance.InstantiateGameObject(PREFAB, parent, false);
		presentationDialogObject.transform.SetParent(parent, true);
		TutorialDialog advisorPresentationDialog = presentationDialogObject.GetComponent<TutorialDialog>();
		presentationDialogObject.gameObject.SetActive(true);
		presentationDialogObject.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return advisorPresentationDialog;
	}

	public static TutorialDialog ShowTutorialTip(string localizationId, Transform parent)
	{
		TutorialDialog advisorPresentationDialog = CreateDialog(parent);
		advisorPresentationDialog.SetTutorialTipDialog(localizationId);
		advisorPresentationDialog.PlayEnterRecipe();
		return advisorPresentationDialog;
	}

	public void Release()
	{
		gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(gameObject, PREFAB);
	}
}

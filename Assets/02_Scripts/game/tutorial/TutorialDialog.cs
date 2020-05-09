using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class TutorialDialog : MonoBehaviour
{
	public static string ADVISO_PRESENTATION_PREFAB = "GUI/TutorialDialog";
	public static string TUTORIAL_TIP_PREFAB = "GUI/TutorialTipDialog";

	public Text presentationText;
	public TutorialDialogChef tutorialDialogChef;
	public AdvisorXmlModel advisorXmlModel;
	public Image advisorIcon;
	public Image background;
	public Image advisorBubble;

	private string shownTutorialDialogPrefab;

	public void SetAdvisorDialog(AdvisorXmlModel advisorXmlModel)
	{
		this.advisorXmlModel = advisorXmlModel;
		LocalizationManager.instance.SetLocalizedText(presentationText, advisorXmlModel.presentation);

		if (!string.IsNullOrEmpty(advisorXmlModel.iconSprite))
		{
			Sprite advisorSprite = Resources.Load<Sprite>(advisorXmlModel.iconSprite);
			advisorIcon.overrideSprite = advisorSprite;
		}

		if (!string.IsNullOrEmpty(advisorXmlModel.bubbleSprite))
		{
			Sprite bubbleSprite = Resources.Load<Sprite>(advisorXmlModel.bubbleSprite);
			advisorBubble.overrideSprite = bubbleSprite;
		}

		if (!string.IsNullOrEmpty(advisorXmlModel.suggestionBackgroundSprite))
		{
			Sprite suggestionBackgroundSprite = Resources.Load<Sprite>(advisorXmlModel.suggestionBackgroundSprite);
			background.overrideSprite = suggestionBackgroundSprite;
		}
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
		tutorialDialogChef.Cook(tutorialDialogChef.onEnterRecipe);
	}

	public static TutorialDialog ShowAdvisorPresentation(AdvisorXmlModel advisorXmlModel, Transform parent)
	{
		TutorialDialog advisorPresentationDialog = CreateDialog(parent, ADVISO_PRESENTATION_PREFAB);
		advisorPresentationDialog.SetAdvisorDialog(advisorXmlModel);
		advisorPresentationDialog.PlayEnterRecipe();
		advisorPresentationDialog.shownTutorialDialogPrefab = ADVISO_PRESENTATION_PREFAB;
		return advisorPresentationDialog;
	}

	private static TutorialDialog CreateDialog(Transform parent, string prefab)
	{
		GameObject presentationDialogObject = GameObjectFactory.instance.InstantiateGameObject(prefab, parent, false);
		presentationDialogObject.transform.SetParent(parent, true);
		TutorialDialog advisorPresentationDialog = presentationDialogObject.GetComponent<TutorialDialog>();
		presentationDialogObject.gameObject.SetActive(true);
		presentationDialogObject.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		return advisorPresentationDialog;
	}

	public static TutorialDialog ShowTutorialTip(string localizationId, Transform parent)
	{
		TutorialDialog tutorialTipDialog = CreateDialog(parent, TUTORIAL_TIP_PREFAB);
		tutorialTipDialog.SetTutorialTipDialog(localizationId);
		tutorialTipDialog.PlayEnterRecipe();
		tutorialTipDialog.shownTutorialDialogPrefab = TUTORIAL_TIP_PREFAB;
		return tutorialTipDialog;
	}

	public void Release()
	{
		gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(gameObject, shownTutorialDialogPrefab);
	}
}

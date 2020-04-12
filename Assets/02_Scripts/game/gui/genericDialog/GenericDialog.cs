using System;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class GenericDialog : MonoBehaviour
{
	public static string OK_BUTTON_LOCALIZATION_ID = "OkText";
	public static string YES_BUTTON_LOCALIZATION_ID = "YesText";
	public static string NO_BUTTON_LOCALIZATION_ID = "NoText";

	public static string PREFAB = "GUI/GenericDialog";

	public GenericDialogChef genericDialogChef;

	public Text titleText;
	public Text messageText;
	public Button okButton;
	public Text okButtonText;
	public Button yesButton;
	public Text yesButtonText;
	public Button noButton;
	public Text noButtonText;

	private EGenericDialogResult result;

	private GenericDialogXmlModel genericDialogXmlModel;

	private void OnEnable()
	{
		result = EGenericDialogResult.NOT_SET;
	}

	public void OnOkButtonSelected()
	{
		result = EGenericDialogResult.OK;
		genericDialogChef.Cook(genericDialogChef.onExitRecipe, OnExitRecipeCompleted);
	}

	public void OnYesButtonSelected()
	{
		result = EGenericDialogResult.YES;
		genericDialogChef.Cook(genericDialogChef.onExitRecipe, OnExitRecipeCompleted);
	}

	public void OnNoButtonSelected()
	{
		result = EGenericDialogResult.NO;
		genericDialogChef.Cook(genericDialogChef.onExitRecipe, OnExitRecipeCompleted);
	}

	private void OnExitRecipeCompleted()
	{
		Debug.Log("Generic Dialog Exit Recipe completed");
	
		SendExitCompletedEvent();

		gameObject.SetActive(false);
		GameObjectFactory.instance.ReleaseGameObject(gameObject, PREFAB);
	}

	private void SendExitCompletedEvent()
	{
		GenericDialogExitCompletedEvent exitCompletedEvent = GenericDialogExitCompletedEvent.CreateInstance(result, genericDialogXmlModel);
		EventMessage exitCompletedEventMessage = new EventMessage(this, exitCompletedEvent);
		exitCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(exitCompletedEventMessage);
	}

	private void SendEnterCompletedEvent()
	{
		GenericDialogEnterCompletedEvent enterCompletedEvent = GenericDialogEnterCompletedEvent.CreateInstance(result, genericDialogXmlModel);
		EventMessage enterCompletedEventMessage = new EventMessage(this, enterCompletedEvent);
		enterCompletedEventMessage.SetMessageType(MessageType.BROADCAST);
		EventMessageManager.instance.QueueMessage(enterCompletedEventMessage);
	}

	private void PlayEnterRecipe()
	{
		genericDialogChef.Cook(genericDialogChef.onEnterRecipe, OnEnterRecipeCompleted);
	}

	private void OnEnterRecipeCompleted()
	{
		Debug.Log("Generic Dialog Enter Recipe completed");
		SendEnterCompletedEvent();
	}

	private void SetDialog(GenericDialogXmlModel genericDialogXmlModel)
	{
		this.genericDialogXmlModel = genericDialogXmlModel;

		okButton.gameObject.SetActive(false);
		yesButton.gameObject.SetActive(false);
		noButton.gameObject.SetActive(false);

		titleText.text = LocalizationManager.instance.GetText(genericDialogXmlModel.title);
		messageText.text = LocalizationManager.instance.GetText(genericDialogXmlModel.message);

		if (genericDialogXmlModel.buttons == 1)
		{
			okButtonText.text = LocalizationManager.instance.GetText(OK_BUTTON_LOCALIZATION_ID);
			okButton.gameObject.SetActive(true);
			return;
		}

		if (genericDialogXmlModel.buttons == 2)
		{
			yesButtonText.text = LocalizationManager.instance.GetText(YES_BUTTON_LOCALIZATION_ID);
			yesButton.gameObject.SetActive(true);

			noButtonText.text = LocalizationManager.instance.GetText(NO_BUTTON_LOCALIZATION_ID);
			noButton.gameObject.SetActive(true);
			return;
		}
	}

	private static GenericDialog CreateGenericDialog(int dialogId, Transform parent)
	{
		GenericDialogXmlModel genericDialogXmlModel = XmlModelManager.instance.FindModel<GenericDialogXmlModel>(dialogId);
		if (genericDialogXmlModel == null)
		{
			return null;
		}

		GameObject genericDialogObject = GameObjectFactory.instance.InstantiateGameObject(PREFAB, parent, false);
		genericDialogObject.transform.SetParent(parent, true);
		GenericDialog genericDialog = genericDialogObject.GetComponent<GenericDialog>();
		genericDialog.SetDialog(genericDialogXmlModel);
		genericDialogObject.gameObject.SetActive(true);
		return genericDialog;
	}

	public static GenericDialog Show(int dialogId, Transform parent)
	{
		GenericDialog genericDialog = CreateGenericDialog(dialogId, parent);
		genericDialog.PlayEnterRecipe();
		return genericDialog;
	}
}

using UnityEngine;
using Messages;

public class QuitGameSessionButton : MonoBehaviour
{
	private static int QUIT_GAME_DIALOG_XML_ID = 5001;
	private GenericDialog quitGameSessionDialog;

	private void OnEnable()
	{
		EventMessageHandler quiDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnQuitDialogExitEvent);
		EventMessageManager.instance.AddHandler(typeof(GenericDialogExitCompletedEvent).Name, quiDialogExitCompletedMessageHandler);
	}

	private void OnDisable()
	{
		quitGameSessionDialog = null;
		EventMessageManager.instance.RemoveHandler(typeof(GenericDialogExitCompletedEvent).Name, this);
	}

	public void OnButtonSelected()
	{
		quitGameSessionDialog = GenericDialog.Show(QUIT_GAME_DIALOG_XML_ID, MainScene.instance.uiWorldCanvas.transform);
	}

	private void OnQuitDialogExitEvent(EventMessage eventMessage)
	{
		GenericDialogExitCompletedEvent genericDialogExitCompletedEvent = eventMessage.eventObject as GenericDialogExitCompletedEvent;
		if (genericDialogExitCompletedEvent.genericDialogXmlModel.id == QUIT_GAME_DIALOG_XML_ID)
		{
			if (genericDialogExitCompletedEvent.result == EGenericDialogResult.YES)
			{
				GameManager.instance.localPlayer.QuitGameSession();
				GameManager.instance.SavePlayer();
				ScenesFlowManager.instance.UnloadingMainScene();
			}
		}
	}
}
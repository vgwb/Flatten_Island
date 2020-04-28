public class TutorialDialogExitCompletedEvent : EventObject
{
	public TutorialDialog tutorialDialog;

	public static TutorialDialogExitCompletedEvent CreateInstance(TutorialDialog tutorialDialog)
	{
		TutorialDialogExitCompletedEvent eventObject = CreateInstance<TutorialDialogExitCompletedEvent>();
		eventObject.name = typeof(TutorialDialogExitCompletedEvent).Name;
		eventObject.tutorialDialog = tutorialDialog;
		return eventObject;
	}
}

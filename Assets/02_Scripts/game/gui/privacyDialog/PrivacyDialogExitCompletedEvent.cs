public class PrivacyDialogExitCompletedEvent : EventObject
{
	public EPrivacyDialogResult result;

	public static PrivacyDialogExitCompletedEvent CreateInstance(EPrivacyDialogResult result)
	{
		PrivacyDialogExitCompletedEvent eventObject = CreateInstance<PrivacyDialogExitCompletedEvent>();
		eventObject.name = typeof(PrivacyDialogExitCompletedEvent).Name;
		eventObject.result = result;
		return eventObject;
	}
}

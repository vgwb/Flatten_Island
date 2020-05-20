public class PrivacyDialogEnterCompletedEvent : EventObject
{
	public EPrivacyDialogResult result;

	public static PrivacyDialogEnterCompletedEvent CreateInstance(EPrivacyDialogResult result)
	{
		PrivacyDialogEnterCompletedEvent eventObject = CreateInstance<PrivacyDialogEnterCompletedEvent>();
		eventObject.name = typeof(PrivacyDialogEnterCompletedEvent).Name;
		eventObject.result = result;
		return eventObject;
	}
}

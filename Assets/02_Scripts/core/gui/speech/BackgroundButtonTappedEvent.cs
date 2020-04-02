public class BackgroundButtonTappedEvent : EventObject
{
	public SpeechMenu speechMenu;
	public static BackgroundButtonTappedEvent CreateInstance(SpeechMenu speechMenu)
	{
		BackgroundButtonTappedEvent eventObject = CreateInstance<BackgroundButtonTappedEvent>();
		eventObject.name = typeof(BackgroundButtonTappedEvent).Name;
		eventObject.speechMenu = speechMenu;
		return eventObject;
	}
}

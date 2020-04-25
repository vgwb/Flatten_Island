public class PlayCreditsCompletedEvent : EventObject
{
	public CreditsMenu creditsMenu;

	public static PlayCreditsCompletedEvent CreateInstance(CreditsMenu creditsMenu)
	{
		PlayCreditsCompletedEvent eventObject = CreateInstance<PlayCreditsCompletedEvent>();
		eventObject.name = typeof(PlayCreditsCompletedEvent).Name;
		eventObject.creditsMenu = creditsMenu;
		return eventObject;
	}
}

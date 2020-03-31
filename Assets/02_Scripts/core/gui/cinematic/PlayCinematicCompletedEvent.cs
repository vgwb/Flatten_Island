public class PlayCinematicCompletedEvent : EventObject
{
	public CinematicMenu cinematicMenu;

	public static PlayCinematicCompletedEvent CreateInstance(CinematicMenu cinematicMenu)
	{
		PlayCinematicCompletedEvent eventObject = CreateInstance<PlayCinematicCompletedEvent>();
		eventObject.name = typeof(PlayCinematicCompletedEvent).Name;
		eventObject.cinematicMenu = cinematicMenu;
		return eventObject;
	}
}

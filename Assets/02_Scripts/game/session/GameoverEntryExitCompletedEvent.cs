public class GameOverEntryExitCompletedEvent : EventObject
{
	public static GameOverEntryExitCompletedEvent CreateInstance()
	{
		GameOverEntryExitCompletedEvent eventObject = CreateInstance<GameOverEntryExitCompletedEvent>();
		eventObject.name = typeof(GameOverEntryExitCompletedEvent).Name;
		return eventObject;
	}
}

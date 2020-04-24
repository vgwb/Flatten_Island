public class GameOverEntryEnterCompletedEvent : EventObject
{
	public static GameOverEntryEnterCompletedEvent CreateInstance()
	{
		GameOverEntryEnterCompletedEvent eventObject = CreateInstance<GameOverEntryEnterCompletedEvent>();
		eventObject.name = typeof(GameOverEntryEnterCompletedEvent).Name;
		return eventObject;
	}
}

public class NextDayEntryEnterCompletedEvent : EventObject
{
	public static NextDayEntryEnterCompletedEvent CreateInstance()
	{
		NextDayEntryEnterCompletedEvent eventObject = CreateInstance<NextDayEntryEnterCompletedEvent>();
		eventObject.name = typeof(NextDayEntryEnterCompletedEvent).Name;
		return eventObject;
	}
}

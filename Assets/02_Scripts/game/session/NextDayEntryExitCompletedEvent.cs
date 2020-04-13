public class NextDayEntryExitCompletedEvent : EventObject
{
	public static NextDayEntryExitCompletedEvent CreateInstance()
	{
		NextDayEntryExitCompletedEvent eventObject = CreateInstance<NextDayEntryExitCompletedEvent>();
		eventObject.name = typeof(NextDayEntryExitCompletedEvent).Name;
		return eventObject;
	}
}

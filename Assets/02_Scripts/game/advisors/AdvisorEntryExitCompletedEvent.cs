public class AdvisorEntryExitCompletedEvent : EventObject
{
	public AdvisorEntry advisorEntry;

	public static AdvisorEntryExitCompletedEvent CreateInstance(AdvisorEntry advisorEntry)
	{
		AdvisorEntryExitCompletedEvent eventObject = CreateInstance<AdvisorEntryExitCompletedEvent>();
		eventObject.name = typeof(AdvisorEntryExitCompletedEvent).Name;
		eventObject.advisorEntry = advisorEntry;
		return eventObject;
	}
}

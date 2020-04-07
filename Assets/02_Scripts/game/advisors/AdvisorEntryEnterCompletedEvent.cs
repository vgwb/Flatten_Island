public class AdvisorEntryEnterCompletedEvent : EventObject
{
	public AdvisorEntry advisorEntry;

	public static AdvisorEntryEnterCompletedEvent CreateInstance(AdvisorEntry advisorEntry)
	{
		AdvisorEntryEnterCompletedEvent eventObject = CreateInstance<AdvisorEntryEnterCompletedEvent>();
		eventObject.name = typeof(AdvisorEntryEnterCompletedEvent).Name;
		eventObject.advisorEntry = advisorEntry;
		return eventObject;
	}
}

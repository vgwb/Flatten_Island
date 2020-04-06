public class AdvisorSelectedEvent : EventObject
{
	public AdvisorEntry advisorEntrySelected;

	public static AdvisorSelectedEvent CreateInstance(AdvisorEntry advisorEntry)
	{
		AdvisorSelectedEvent eventObject = CreateInstance<AdvisorSelectedEvent>();
		eventObject.name = typeof(AdvisorSelectedEvent).Name;
		eventObject.advisorEntrySelected = advisorEntry;
		return eventObject;
	}
}

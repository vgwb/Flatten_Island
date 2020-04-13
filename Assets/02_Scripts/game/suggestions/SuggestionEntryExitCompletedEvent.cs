public class SuggestionEntryExitCompletedEvent : EventObject
{
	public SuggestionOptionXmlModel selectedSuggestionOption;

	public static SuggestionEntryExitCompletedEvent CreateInstance(SuggestionOptionXmlModel selectedSuggestionOption)
	{
		SuggestionEntryExitCompletedEvent eventObject = CreateInstance<SuggestionEntryExitCompletedEvent>();
		eventObject.name = typeof(SuggestionEntryExitCompletedEvent).Name;
		eventObject.selectedSuggestionOption = selectedSuggestionOption;
		return eventObject;
	}
}

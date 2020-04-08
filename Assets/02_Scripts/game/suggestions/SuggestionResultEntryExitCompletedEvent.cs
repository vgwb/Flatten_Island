public class SuggestionResultEntryExitCompletedEvent : EventObject
{
	public SuggestionResultEntry suggestionResultEntry;
	public SuggestionOptionXmlModel selectedSuggestionOptionXmlModel;

	public static SuggestionResultEntryExitCompletedEvent CreateInstance(SuggestionResultEntry suggestionResultEntry, SuggestionOptionXmlModel selectedSuggestionOptionXmlModel)
	{
		SuggestionResultEntryExitCompletedEvent eventObject = CreateInstance<SuggestionResultEntryExitCompletedEvent>();
		eventObject.name = typeof(SuggestionResultEntryExitCompletedEvent).Name;
		eventObject.suggestionResultEntry = suggestionResultEntry;
		eventObject.selectedSuggestionOptionXmlModel = selectedSuggestionOptionXmlModel;
		return eventObject;
	}
}

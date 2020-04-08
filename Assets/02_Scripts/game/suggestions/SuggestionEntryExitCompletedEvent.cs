public class SuggestionEntryExitCompletedEvent : EventObject
{
	public SuggestionEntry suggestionEntry;

	public static SuggestionEntryExitCompletedEvent CreateInstance(SuggestionEntry suggestionEntry)
	{
		SuggestionEntryExitCompletedEvent eventObject = CreateInstance<SuggestionEntryExitCompletedEvent>();
		eventObject.name = typeof(SuggestionEntryExitCompletedEvent).Name;
		eventObject.suggestionEntry = suggestionEntry;
		return eventObject;
	}
}

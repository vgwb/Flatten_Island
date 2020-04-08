public class SuggestionEntryEnterCompletedEvent : EventObject
{
	public SuggestionEntry suggestionEntry;

	public static SuggestionEntryEnterCompletedEvent CreateInstance(SuggestionEntry suggestionEntry)
	{
		SuggestionEntryEnterCompletedEvent eventObject = CreateInstance<SuggestionEntryEnterCompletedEvent>();
		eventObject.name = typeof(SuggestionEntryEnterCompletedEvent).Name;
		eventObject.suggestionEntry = suggestionEntry;
		return eventObject;
	}
}

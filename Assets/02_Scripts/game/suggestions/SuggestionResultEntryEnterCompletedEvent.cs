public class SuggestionResultEntryEnterCompletedEvent : EventObject
{
	public SuggestionResultEntry suggestionResultEntry;

	public static SuggestionResultEntryEnterCompletedEvent CreateInstance(SuggestionResultEntry suggestionResultEntry)
	{
		SuggestionResultEntryEnterCompletedEvent eventObject = CreateInstance<SuggestionResultEntryEnterCompletedEvent>();
		eventObject.name = typeof(SuggestionResultEntryEnterCompletedEvent).Name;
		eventObject.suggestionResultEntry = suggestionResultEntry;
		return eventObject;
	}
}

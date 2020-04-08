public class SuggestionResultEntryExitCompletedEvent : EventObject
{
	public SuggestionResultEntry suggestionResultEntry;

	public static SuggestionResultEntryExitCompletedEvent CreateInstance(SuggestionResultEntry suggestionResultEntry)
	{
		SuggestionResultEntryExitCompletedEvent eventObject = CreateInstance<SuggestionResultEntryExitCompletedEvent>();
		eventObject.name = typeof(SuggestionResultEntryExitCompletedEvent).Name;
		eventObject.suggestionResultEntry = suggestionResultEntry;
		return eventObject;
	}
}

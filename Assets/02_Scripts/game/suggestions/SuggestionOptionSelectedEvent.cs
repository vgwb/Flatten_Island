public class SuggestionOptionSelectedEvent : EventObject
{
	public SuggestionOptionXmlModel suggestionOptionXmlModel;

	public static SuggestionOptionSelectedEvent CreateInstance(SuggestionOptionXmlModel suggestionOptionXmlModel)
	{
		SuggestionOptionSelectedEvent eventObject = CreateInstance<SuggestionOptionSelectedEvent>();
		eventObject.name = typeof(SuggestionOptionSelectedEvent).Name;
		eventObject.suggestionOptionXmlModel = suggestionOptionXmlModel;
		return eventObject;
	}
}

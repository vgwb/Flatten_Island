public class LanguageSelectedEvent : EventObject
{
	public LocalizationXmlModel languageXmlModel;

	public static LanguageSelectedEvent CreateInstance(LocalizationXmlModel languageXmlModel)
	{
		LanguageSelectedEvent eventObject = CreateInstance<LanguageSelectedEvent>();
		eventObject.name = typeof(LanguageSelectedEvent).Name;
		eventObject.languageXmlModel = languageXmlModel;
		return eventObject;
	}
}

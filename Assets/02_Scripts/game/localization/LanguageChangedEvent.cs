public class LanguageChangedEvent : EventObject
{
	public LocalizationXmlModel languageXmlModel;

	public static LanguageChangedEvent CreateInstance(LocalizationXmlModel languageXmlModel)
	{
		LanguageChangedEvent eventObject = CreateInstance<LanguageChangedEvent>();
		eventObject.name = typeof(LanguageChangedEvent).Name;
		eventObject.languageXmlModel = languageXmlModel;
		return eventObject;
	}
}

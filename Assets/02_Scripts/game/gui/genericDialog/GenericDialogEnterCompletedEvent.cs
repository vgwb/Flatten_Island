public class GenericDialogEnterCompletedEvent : EventObject
{
	public EGenericDialogResult result;
	public GenericDialogXmlModel genericDialogXmlModel;

	public static GenericDialogEnterCompletedEvent CreateInstance(EGenericDialogResult result, GenericDialogXmlModel genericDialogXmlModel)
	{
		GenericDialogEnterCompletedEvent eventObject = CreateInstance<GenericDialogEnterCompletedEvent>();
		eventObject.name = typeof(GenericDialogEnterCompletedEvent).Name;
		eventObject.result = result;
		eventObject.genericDialogXmlModel = genericDialogXmlModel;
		return eventObject;
	}
}

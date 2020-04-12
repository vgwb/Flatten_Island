public class GenericDialogExitCompletedEvent : EventObject
{
	public EGenericDialogResult result;
	public GenericDialogXmlModel genericDialogXmlModel;

	public static GenericDialogExitCompletedEvent CreateInstance(EGenericDialogResult result, GenericDialogXmlModel genericDialogXmlModel)
	{
		GenericDialogExitCompletedEvent eventObject = CreateInstance<GenericDialogExitCompletedEvent>();
		eventObject.name = typeof(GenericDialogExitCompletedEvent).Name;
		eventObject.result = result;
		eventObject.genericDialogXmlModel = genericDialogXmlModel;
		return eventObject;
	}
}

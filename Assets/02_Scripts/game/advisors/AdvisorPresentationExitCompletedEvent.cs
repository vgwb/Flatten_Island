public class AdvisorPresentationExitCompletedEvent : EventObject
{
	public AdvisorPresentationDialog advisorPresentation;

	public static AdvisorPresentationExitCompletedEvent CreateInstance(AdvisorPresentationDialog advisorPresentation)
	{
		AdvisorPresentationExitCompletedEvent eventObject = CreateInstance<AdvisorPresentationExitCompletedEvent>();
		eventObject.name = typeof(AdvisorPresentationExitCompletedEvent).Name;
		eventObject.advisorPresentation = advisorPresentation;
		return eventObject;
	}
}

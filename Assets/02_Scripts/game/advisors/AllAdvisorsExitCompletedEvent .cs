public class AllAdvisorsExitCompletedEvent : EventObject
{
	public AdvisorXmlModel selectedAdvisor;

	public static AllAdvisorsExitCompletedEvent CreateInstance(AdvisorXmlModel selectedAdvisor)
	{
		AllAdvisorsExitCompletedEvent eventObject = CreateInstance<AllAdvisorsExitCompletedEvent>();
		eventObject.name = typeof(AllAdvisorsExitCompletedEvent).Name;
		eventObject.selectedAdvisor = selectedAdvisor;
		return eventObject;
	}
}

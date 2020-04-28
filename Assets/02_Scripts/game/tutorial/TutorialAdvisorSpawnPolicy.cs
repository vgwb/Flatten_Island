using System.Collections.Generic;

public class TutorialAdvisorSpawnPolicy : IAdvisorSpawnPolicy
{
	private Queue<AdvisorXmlModel> advisorsQueue;

	public void Initialize()
	{
		InitializeQueue();
	}

	private void InitializeQueue()
	{
		if (advisorsQueue == null)
		{
			advisorsQueue = new Queue<AdvisorXmlModel>();
		}
		else
		{
			advisorsQueue.Clear();
		}

		AdvisorXmlModel prAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(FlattenIslandGameConstants.PR_ADVISOR_ID);
		advisorsQueue.Enqueue(prAdvisor);

		AdvisorXmlModel treasurerAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(FlattenIslandGameConstants.TREASURER_ADVISOR_ID);
		advisorsQueue.Enqueue(treasurerAdvisor);

		AdvisorXmlModel labScientistAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(FlattenIslandGameConstants.LAB_SCIENTIST_ADVISOR_ID);
		advisorsQueue.Enqueue(labScientistAdvisor);

		AdvisorXmlModel hospitalDoctorAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(FlattenIslandGameConstants.HOSPITAL_DOCTOR_ADVISOR_ID);
		advisorsQueue.Enqueue(hospitalDoctorAdvisor);

		AdvisorXmlModel commanderAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(FlattenIslandGameConstants.COMMANDER_ADVISOR_ID);
		advisorsQueue.Enqueue(commanderAdvisor);
	}

	public List<AdvisorXmlModel> GetAdvisors()
	{
		List<AdvisorXmlModel> advisors = new List<AdvisorXmlModel>();

		AdvisorXmlModel advisorXmlModel = advisorsQueue.Dequeue();
		if (advisorXmlModel != null)
		{
			advisors.Add(advisorXmlModel);
		}

		return advisors;
	}

	public List<AdvisorXmlModel> GetAdvisors(List<AdvisorXmlModel> advisorsToAvoid)
	{
		return GetAdvisors();
	}
}
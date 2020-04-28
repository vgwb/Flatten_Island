using System.Collections.Generic;

public class TutorialAdvisorSpawnPolicy : IAdvisorSpawnPolicy
{
	private const int PR_ADVISOR_ID = 2001;
	private const int TREASURER_ADVISOR_ID = 2002;
	private const int LAB_SCIENTIST_ADVISOR_ID = 2003;
	private const int HOSPITAL_DOCTOR_ADVISOR_ID = 2004;
	private const int COMMANDER_ADVISOR_ID = 2005;

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

		AdvisorXmlModel prAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(PR_ADVISOR_ID);
		advisorsQueue.Enqueue(prAdvisor);

		AdvisorXmlModel treasurerAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(TREASURER_ADVISOR_ID);
		advisorsQueue.Enqueue(treasurerAdvisor);

		AdvisorXmlModel labScientistAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(LAB_SCIENTIST_ADVISOR_ID);
		advisorsQueue.Enqueue(labScientistAdvisor);

		AdvisorXmlModel hospitalDoctorAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(HOSPITAL_DOCTOR_ADVISOR_ID);
		advisorsQueue.Enqueue(hospitalDoctorAdvisor);

		AdvisorXmlModel commanderAdvisor = XmlModelManager.instance.FindModel<AdvisorXmlModel>(COMMANDER_ADVISOR_ID);
		advisorsQueue.Enqueue(commanderAdvisor);

		RemoveAlradyShownAdvisors();
	}

	private void RemoveAlradyShownAdvisors()
	{
		LocalPlayer localPlayer = GameManager.instance.localPlayer;
		for (int i = 1; i <= localPlayer.gameSession.day; i++)
		{
			if (advisorsQueue.Count > 0)
			{
				advisorsQueue.Dequeue();
			}
		}
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
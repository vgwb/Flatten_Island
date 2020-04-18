using System.Collections.Generic;

public class AdvisorRandomSpawnPolicy : IAdvisorSpawnPolicy
{
	public const int NUMBER_OF_ADVISORS = 3;

	private ShuffleBag<AdvisorXmlModel> advisorsShuffleBag;
	private List<AdvisorXmlModel> advisorModels;

	public void Initialize()
	{
		advisorModels = XmlModelManager.instance.FindModels<AdvisorXmlModel>();
		Reset();
	}

	public void Reset()
	{
		InitializeShuffleBag();
	}

	private void InitializeShuffleBag()
	{
		if (advisorsShuffleBag == null)
		{
			advisorsShuffleBag = new ShuffleBag<AdvisorXmlModel>(advisorModels.Count);
		}
		else
		{
			advisorsShuffleBag.Clear();
		}

		foreach (AdvisorXmlModel advisorModel in advisorModels)
		{
			advisorsShuffleBag.Add(advisorModel, 1);
		}

		advisorsShuffleBag.Shuffle();
	}

	public List<AdvisorXmlModel> GetAdvisors()
	{
		List<AdvisorXmlModel> advisors = new List<AdvisorXmlModel>();

		while (advisors.Count < NUMBER_OF_ADVISORS)
		{
			AdvisorXmlModel pickedAdvisor = advisorsShuffleBag.Next();

			if (!advisors.Contains(pickedAdvisor))
			{
				advisors.Add(pickedAdvisor);
			}
		}

		return advisors;
	}

	public List<AdvisorXmlModel> GetAdvisors(List<AdvisorXmlModel> advisorsToAvoid)
	{
		if (advisorsToAvoid == null)
		{
			return GetAdvisors();
		}

		if (!HasEnoughAdvisorsToPick(advisorsToAvoid.Count))
		{
			return GetAdvisors();
		}

		List<AdvisorXmlModel> advisors = new List<AdvisorXmlModel>();
		while (advisors.Count < NUMBER_OF_ADVISORS)
		{
			AdvisorXmlModel pickedAdvisor = advisorsShuffleBag.Next();

			if (!advisorsToAvoid.Contains(pickedAdvisor) && !advisors.Contains(pickedAdvisor))
			{
				advisors.Add(pickedAdvisor);
			}
		}

		return advisors;
	}

	private bool HasEnoughAdvisorsToPick(int advisorsToAvoidCount)
	{
		return advisorsToAvoidCount + NUMBER_OF_ADVISORS < advisorModels.Count;
	}
}
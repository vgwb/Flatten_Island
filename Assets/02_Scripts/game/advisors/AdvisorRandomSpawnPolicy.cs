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
		for (int i = 0; i < NUMBER_OF_ADVISORS; i++)
		{
			advisors.Add(advisorsShuffleBag.Next());
		}

		return advisors;
	}
}
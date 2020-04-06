using UnityEngine;
using System.Collections.Generic;

public class AdvisorsManager : MonoSingleton
{
	public static AdvisorsManager instance
	{
		get
		{
			return GetInstance<AdvisorsManager>();
		}
	}

	private LocalPlayer localPlayer;
	private IAdvisorSpawnPolicy advisorSpawnPolicy;

	private List<AdvisorXmlModel> currentAdvisors;

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();
		localPlayer = GameManager.instance.localPlayer;
		currentAdvisors = new List<AdvisorXmlModel>();
		advisorSpawnPolicy = new AdvisorRandomSpawnPolicy();
		advisorSpawnPolicy.Initialize();

		//should check here if we have to load advisors from save game
	}

	public List<AdvisorXmlModel> GetCurrentAdvisors()
	{
		if (currentAdvisors.Count == 0)
		{
			currentAdvisors = new List<AdvisorXmlModel>(advisorSpawnPolicy.GetAdvisors());
		}

		return currentAdvisors;
	}

	public void DiscardCurrentAdvisors()
	{
		currentAdvisors.Clear();
	}
}
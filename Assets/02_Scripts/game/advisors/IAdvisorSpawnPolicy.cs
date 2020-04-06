using System.Collections.Generic;

public interface IAdvisorSpawnPolicy
{
	void Initialize();
	void Reset();

	List<AdvisorXmlModel> GetAdvisors();
}
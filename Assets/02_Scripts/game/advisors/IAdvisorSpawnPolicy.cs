using System.Collections.Generic;

public interface IAdvisorSpawnPolicy
{
	void Initialize();

	List<AdvisorXmlModel> GetAdvisors();
	List<AdvisorXmlModel> GetAdvisors(List<AdvisorXmlModel> advisorsToAvoid);
}
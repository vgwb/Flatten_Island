using System.Collections.Generic;

public interface IGameSimulatorStrategy
{
	void Initialize(GameSession gameSession);
	AdvisorXmlModel ChoseAdvisor(List<AdvisorXmlModel> advisorsAvailable);
	string GetName();
}
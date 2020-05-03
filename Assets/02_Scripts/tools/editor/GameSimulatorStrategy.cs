using System.Collections.Generic;

public interface IGameSimulatorStrategy
{
	void Initialize(GameSession gameSession, GameSessionSimulatorSettings simulatorSettings);
	AdvisorXmlModel ChoseAdvisor(List<AdvisorXmlModel> advisorsAvailable);
	string GetLogDescription();
}
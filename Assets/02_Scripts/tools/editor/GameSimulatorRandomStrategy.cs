using System;
using System.Collections.Generic;

public class GameSimulatorRandomStrategy: IGameSimulatorStrategy
{
	private GameSession gameSession;

	public void Initialize(GameSession gameSession)
	{
		this.gameSession = gameSession;
	}

	public AdvisorXmlModel ChoseAdvisor(List<AdvisorXmlModel> advisorsAvailable)
	{
		int randomAdvisorIndex = RandomGenerator.GetRandom(0, advisorsAvailable.Count);
		return advisorsAvailable[randomAdvisorIndex];
	}

	public string GetLogDescription()
	{
		return "Random";
	}
}
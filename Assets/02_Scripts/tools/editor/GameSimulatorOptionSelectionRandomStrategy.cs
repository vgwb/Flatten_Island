using System;
using System.Collections.Generic;

public class GameSimulatorOptionSelectionRandomStrategy : IGameSimulatorOptionSelectionStrategy
{
	private GameSession gameSession;

	public void Initialize(GameSession gameSession, GameSessionSimulatorSettings simulatorSettings)
	{
		this.gameSession = gameSession;
	}

	public SuggestionOptionXmlModel ChoseOption(AdvisorXmlModel advisorXmlModel, List<SuggestionOptionXmlModel> optionsAvailable)
	{
		int randomIndex = RandomGenerator.GetRandom(0, optionsAvailable.Count);
		return optionsAvailable[randomIndex];
	}

	public string GetLogDescription()
	{
		return "Random";
	}
}
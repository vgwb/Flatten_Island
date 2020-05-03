using System.Collections.Generic;

public interface IGameSimulatorOptionSelectionStrategy
{
	void Initialize(GameSession gameSession, GameSessionSimulatorSettings simulatorSettings);
	SuggestionOptionXmlModel ChoseOption(AdvisorXmlModel advisorXmlModel, List<SuggestionOptionXmlModel> optionsAvailable);
	string GetLogDescription();
}
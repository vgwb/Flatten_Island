using System.Collections.Generic;

public interface IGameSimulatorOptionSelectionStrategy
{
	void Initialize(GameSession gameSession);
	SuggestionOptionXmlModel ChoseOption(List<SuggestionOptionXmlModel> optionsAvailable);
	string GetLogDescription();
}
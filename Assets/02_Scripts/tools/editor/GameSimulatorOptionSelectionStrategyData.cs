public class GameSimulatorOptionSelectionStrategyData
{
	public IGameSimulatorOptionSelectionStrategy strategy;
	public string probabilityText;

	public GameSimulatorOptionSelectionStrategyData(IGameSimulatorOptionSelectionStrategy strategy, string probabilityText)
	{
		this.strategy = strategy;
		this.probabilityText = probabilityText;
	}
}

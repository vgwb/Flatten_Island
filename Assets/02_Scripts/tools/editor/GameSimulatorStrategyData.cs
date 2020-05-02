public class GameSimulatorStrategyData
{
	public IGameSimulatorStrategy strategy;
	public string probabilityText;

	public GameSimulatorStrategyData(IGameSimulatorStrategy strategy, string probabilityText)
	{
		this.strategy = strategy;
		this.probabilityText = probabilityText;
	}
}

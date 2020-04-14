using System.Xml.Linq;

public class GamePhaseGrowthRateConditionXmlModel : GamePhaseEndConditionXmlModel
{
	private int minGrowthRate;
	private int maxGrowthRate;

	public GamePhaseGrowthRateConditionXmlModel()
	{

	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);

		minGrowthRate = ParseIntAttribute(element, "minGrowthRate", int.MinValue);
		maxGrowthRate = ParseIntAttribute(element, "maxGrowthRate", int.MaxValue);
	}

	public override bool IsSatisfied(GameSession gameSession)
	{
		return (gameSession.growthRate < minGrowthRate || gameSession.growthRate > maxGrowthRate);
	}
}

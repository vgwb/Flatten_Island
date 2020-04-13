using System.Xml.Linq;

public class GamePhaseGrowthRateConditionXmlModel : GamePhaseEndConditionXmlModel
{
	private int growthRate;

	public GamePhaseGrowthRateConditionXmlModel()
	{

	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);

		growthRate = ParseIntAttribute(element, "growthRate");
	}

	public override bool IsSatisfied(GameSession gameSession)
	{
		return (gameSession.growthRate < growthRate);
	}
}

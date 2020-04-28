using System.Xml.Linq;

public class GamePhaseDurationConditionXmlModel : GamePhaseEndConditionXmlModel
{
	private int days;

	public GamePhaseDurationConditionXmlModel()
	{

	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);

		days = ParseIntAttribute(element, "days");
	}


	public override bool IsSatisfied(GameSession gameSession)
	{
		IGamePhase gamePhase = gameSession.gamePhase;
		return (gameSession.day - gamePhase.GetStartDay() >= days);
	}
}

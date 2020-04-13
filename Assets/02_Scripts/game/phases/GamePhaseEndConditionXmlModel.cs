using System.Xml.Linq;

public abstract class GamePhaseEndConditionXmlModel : XmlModel
{
	public GamePhaseEndConditionXmlModel()
	{

	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);
	}

	public abstract bool IsSatisfied(GameSession gameSession);
}

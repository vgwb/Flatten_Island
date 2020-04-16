using System.Collections.Generic;
using System.Xml.Linq;

public class GamePhaseRequirementXmlModel : RequirementXmlModel
{
	private List<int> gamePhaseIds;

	public GamePhaseRequirementXmlModel()
	{

	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);

		gamePhaseIds = ParseIntsFromChildElement(element, "phaseId");
	}

	public override bool IsSatisfied()
	{
		if (gamePhaseIds.Count == 0)
		{
			return true;
		}

		GameSession gameSession = GameManager.instance.localPlayer.gameSession;

		return gamePhaseIds.Contains(gameSession.gamePhase.GetPhaseId());
	}
}

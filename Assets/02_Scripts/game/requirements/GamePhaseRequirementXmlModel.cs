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

	public override bool IsSatisfied(RequirementContext requirementContext)
	{
		if (gamePhaseIds.Count == 0)
		{
			return true;
		}

		GameRequirementContext gameRequirementContext = requirementContext as GameRequirementContext;
		return gamePhaseIds.Contains(gameRequirementContext.localPlayer.gameSession.gamePhase.GetPhaseId());
	}
}

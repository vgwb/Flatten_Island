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
		throw new System.NotImplementedException();
	}
}

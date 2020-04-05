using System.Collections.Generic;
using System.Xml.Linq;

public class GameStoryRequirementXmlModel : RequirementXmlModel
{
	private List<int> gameStoryIds;

	public GameStoryRequirementXmlModel()
	{

	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);

		gameStoryIds = ParseIntsFromChildElement(element, "storyId");
	}


	public override bool IsSatisfied()
	{
		throw new System.NotImplementedException();
	}
}

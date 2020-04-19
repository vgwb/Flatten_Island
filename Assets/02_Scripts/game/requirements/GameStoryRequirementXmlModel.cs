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

	public override bool IsSatisfied(RequirementContext requirementContext)
	{
		if (gameStoryIds.Count == 0)
		{
			return true;
		}

		GameRequirementContext gameRequirementContext = requirementContext as GameRequirementContext;

		GameSession gameSession = gameRequirementContext.localPlayer.gameSession;

		foreach (GameStoryXmlModel gameStoryXmlModel in gameSession.activeGameStories)
		{
			if (gameStoryIds.Contains(gameStoryXmlModel.id))
			{
				return true;
			}
		}

		return false;
	}
}

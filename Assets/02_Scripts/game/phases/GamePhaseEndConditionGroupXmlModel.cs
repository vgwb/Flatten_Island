using System.Collections.Generic;
using System.Xml.Linq;

public class GamePhaseEndConditionGroupXmlModel : GamePhaseEndConditionXmlModel
{
	public const string ELEMENT_NAME = "endConditions";

	public List<GamePhaseEndConditionXmlModel> gamePhaseEndConditionsXmlModels;

	public GamePhaseEndConditionGroupXmlModel()
	{
		gamePhaseEndConditionsXmlModels = new List<GamePhaseEndConditionXmlModel>();
	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);

		foreach (XElement childElement in element.Elements())
		{
			if (childElement != null)
			{
				GamePhaseEndConditionXmlModel gamePhaseEndConditionXmlModel = ParseGamePhaseEndConditionXmlElement(childElement);
				if (gamePhaseEndConditionXmlModel != null)
				{
					gamePhaseEndConditionsXmlModels.Add(gamePhaseEndConditionXmlModel);
				}
			}
		}
	}

	public override bool IsSatisfied(GameSession gameSession)
	{
		for (int i = 0, n = gamePhaseEndConditionsXmlModels.Count; i < n; i++)
		{
			if (!gamePhaseEndConditionsXmlModels[i].IsSatisfied(gameSession))
			{
				return false;
			}
		}
		return true;
	}

	public GamePhaseEndConditionXmlModel ParseGamePhaseEndConditionXmlElement(XElement element)
	{
		GamePhaseEndConditionXmlModel gamePhaseEndConditionXmlModel = XmlModelTypeMappings.instance.CreateInstanceFromName(element.Name.ToString()) as GamePhaseEndConditionXmlModel;
		if (gamePhaseEndConditionXmlModel != null)
		{
			gamePhaseEndConditionXmlModel.Initialize(element);
		}

		return gamePhaseEndConditionXmlModel;
	}
}

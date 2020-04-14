using System.Xml.Linq;

public class GamePhaseXmlModel : XmlModel
{
	public string description;
	public int nextPhaseId;
	public string musicAudioClip;

	public GamePhaseEndConditionGroupXmlModel endConditions;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		description = ParseStringAttribute(element, "description");
		musicAudioClip = ParseStringAttribute(element, "musicAudioClip");
		nextPhaseId = ParseIntAttribute(element, "nextPhaseId");
		endConditions = ParseGamePhaseEndConditionsGroupFromChildElement(element, "endConditions");
	}

	public static GamePhaseEndConditionGroupXmlModel ParseGamePhaseEndConditionsGroupFromChildElement(XElement element, string childElementName)
	{
		XElement childElement = element.Element(childElementName);
		if (childElement != null)
		{
			GamePhaseEndConditionGroupXmlModel gamePhaseEndConditionGroupXmlModel = new GamePhaseEndConditionGroupXmlModel();
			gamePhaseEndConditionGroupXmlModel.Initialize(childElement);
			return gamePhaseEndConditionGroupXmlModel;
		}
		return null;
	}
}

using System.Xml.Linq;

public class GamePhaseXmlModel : XmlModel
{
	public string description;
	public int nextPhaseId;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		description = ParseStringAttribute(element, "description");
		nextPhaseId = ParseIntAttribute(element, "nextPhaseId");
	}
}

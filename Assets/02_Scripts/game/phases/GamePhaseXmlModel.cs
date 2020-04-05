using System.Xml.Linq;

public class GamePhaseXmlModel : XmlModel
{
	public string description;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		description = ParseStringAttribute(element, "description");
	}
}

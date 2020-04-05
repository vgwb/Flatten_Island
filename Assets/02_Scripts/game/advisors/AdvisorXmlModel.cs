using System.Xml.Linq;

public class AdvisorXmlModel : XmlModel
{
	public string iconTexture;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		iconTexture = ParseStringAttribute(element, "iconTexture");
	}
}

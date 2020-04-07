using System.Xml.Linq;

public class AdvisorXmlModel : XmlModel
{
	public string portraitSprite;
	public string iconSprite;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		portraitSprite = ParseStringAttribute(element, "portraitSprite");
		iconSprite = ParseStringAttribute(element, "iconSprite");
	}
}

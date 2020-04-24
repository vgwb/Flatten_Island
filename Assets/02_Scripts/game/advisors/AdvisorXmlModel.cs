using System.Xml.Linq;

public class AdvisorXmlModel : XmlModel
{
	public string portraitSprite;
	public string portraitFullSprite;
	public string iconSprite;
	public string portraitBackgroundSprite;
	public string bubbleSprite;
	public string suggestionBackgroundSprite;
	public string suggestionResultBackgroundSprite;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		portraitSprite = ParseStringAttribute(element, "portraitSprite");
		portraitFullSprite = ParseStringAttribute(element, "portraitFullSprite");
		iconSprite = ParseStringAttribute(element, "iconSprite");
		portraitBackgroundSprite = ParseStringAttribute(element, "portraitBackgroundSprite");
		bubbleSprite = ParseStringAttribute(element, "bubbleSprite");
		suggestionBackgroundSprite = ParseStringAttribute(element, "suggestionBackgroundSprite");
		suggestionResultBackgroundSprite = ParseStringAttribute(element, "suggestionResultBackgroundSprite");

	}
}

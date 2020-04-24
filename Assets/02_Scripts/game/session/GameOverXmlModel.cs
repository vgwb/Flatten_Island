using System.Xml.Linq;

public class GameOverXmlModel : XmlModel
{
	public string winTitle;
	public string winDescription;
	public string loseTitle;
	public string loseDescription;
	public string winButton;
	public string loseButton;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		winTitle = ParseStringAttribute(element, "winTitle");
		winDescription = ParseStringAttribute(element, "winDescription");
		winButton = ParseStringAttribute(element, "winButton");
		loseTitle = ParseStringAttribute(element, "loseTitle");
		loseDescription = ParseStringAttribute(element, "loseDescription");
		loseButton = ParseStringAttribute(element, "loseButton");

	}
}

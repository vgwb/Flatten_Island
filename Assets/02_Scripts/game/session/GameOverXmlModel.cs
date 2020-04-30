using System.Xml.Linq;

public class GameOverXmlModel : XmlModel
{
	public string winTitle;
	public string winDescription;
	public string loseCapacityTitle;
	public string loseCapacityDescription;
	public string loseMoneyTitle;
	public string loseMoneyDescription;
	public string losePublicOpinionTitle;
	public string losePublicOpinionDescription;
	public string winButton;
	public string loseButton;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		winTitle = ParseStringAttribute(element, "winTitle");
		winDescription = ParseStringAttribute(element, "winDescription");
		winButton = ParseStringAttribute(element, "winButton");
		loseCapacityTitle = ParseStringAttribute(element, "loseCapacityTitle");
		loseCapacityDescription = ParseStringAttribute(element, "loseCapacityDescription");
		loseMoneyTitle = ParseStringAttribute(element, "loseMoneyTitle");
		loseMoneyDescription = ParseStringAttribute(element, "loseMoneyDescription");
		losePublicOpinionTitle = ParseStringAttribute(element, "losePublicOpinionTitle");
		losePublicOpinionDescription = ParseStringAttribute(element, "losePublicOpinionDescription");
		loseButton = ParseStringAttribute(element, "loseButton");
	}
}

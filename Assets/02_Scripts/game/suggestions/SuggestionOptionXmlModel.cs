using System.Xml.Linq;

public class SuggestionOptionXmlModel : XmlModel
{
	public string text;
	public int moneyModifier;
	public int capacityModifier;
	public int publicOpinionModifier;
	public int growthRateModifier;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);

		text = ParseStringAttribute(element, "text");
		moneyModifier = ParseIntAttribute(element, "money");
		capacityModifier = ParseIntAttribute(element, "capacity");
		publicOpinionModifier = ParseIntAttribute(element, "publicOpinion");
		growthRateModifier = ParseIntAttribute(element, "growthRate");
	}
}

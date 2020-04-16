using System.Xml.Linq;

public class SuggestionOptionXmlModel : XmlModel
{
	public string text;
	public int moneyModifier;
	public int capacityModifier;
	public int publicOpinionModifier;
	public int growthRateModifier;
	public int vaccineModifier;
	public int startStoryId;
	public int stopStoryId;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);

		text = ParseStringAttribute(element, "text");
		moneyModifier = ParseIntAttribute(element, "money");
		capacityModifier = ParseIntAttribute(element, "capacity");
		publicOpinionModifier = ParseIntAttribute(element, "publicOpinion");
		growthRateModifier = ParseIntAttribute(element, "growthRate");
		vaccineModifier = ParseIntAttribute(element, "vaccine");
		startStoryId = ParseIntAttribute(element, "startStoryId");
		stopStoryId = ParseIntAttribute(element, "stopStoryId");
	}
}

using System.Xml.Linq;

public class LocalizationXmlModel : XmlModel
{
	public string languageId;
	public bool useLatinFont;
	public bool isRightToLeft;
	public string[] tags;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		languageId = ParseStringAttribute(element, "languageId");
		tags = ParseStringsFromChildElement(element, "tag");
		useLatinFont = ParseBoolAttribute(element, "useLatinFont");
		isRightToLeft = ParseBoolAttribute(element, "rtl", false);
	}
}

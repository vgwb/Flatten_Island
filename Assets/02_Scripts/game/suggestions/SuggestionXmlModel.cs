using System.Collections.Generic;
using System.Xml.Linq;

public class SuggestionXmlModel : XmlModel
{
	public int advisorId;
	public string title;
	public List<SuggestionOptionXmlModel> suggestionOptionsList;
	public RequirementGroupXmlModel requirements;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		advisorId = ParseIntAttribute(element, "advisorId");
		title = ParseStringAttribute(element, "title");

		XElement suggestionOptionsElement = element.Element("suggestionOptions");
		suggestionOptionsList = ParseModelsFromChildElement<SuggestionOptionXmlModel>(suggestionOptionsElement, "suggestionOption");
		requirements = ParseRequirementGroupFromChildElement(element, "requirements");
	}
}

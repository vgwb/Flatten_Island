using System.Collections.Generic;
using System.Xml.Linq;

public class SuggestionXmlModel : XmlModel
{
	public int advisorId;
	public string title;
	public string description;
	public List<SuggestionOptionXmlModel> suggestionOptionsList;
	public RequirementGroupXmlModel requirements;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		advisorId = ParseIntAttribute(element, "advisorId");
		title = ParseStringAttribute(element, "title");
		description = ParseStringAttribute(element, "description");

		XElement suggestionOptionsElement = element.Element("suggestionOptions");
		suggestionOptionsList = ParseModelsFromChildElement<SuggestionOptionXmlModel>(suggestionOptionsElement, "suggestionOption");
		requirements = ParseRequirementGroupFromChildElement(element, "requirements");
	}

	public bool IsAvailable(GameSession gameSession)
	{
		if (requirements == null)
		{
			return true;
		}

		if (requirements.IsSatisfied())
		{
			return true;
		}

		return false;
	}
}

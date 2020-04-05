using System.Collections.Generic;
using System.Xml.Linq;

public class RequirementGroupXmlModel : RequirementXmlModel
{
	public const string ELEMENT_NAME = "requirements";

	public List<RequirementXmlModel> requirementXmlModels;

	public RequirementGroupXmlModel()
	{
		requirementXmlModels = new List<RequirementXmlModel>();
	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);

		foreach (XElement childElement in element.Elements())
		{
			if (childElement != null)
			{
				RequirementXmlModel requirementXmlModel = ParseRequirementXmlElement(childElement);
				if (requirementXmlModel != null)
				{
					requirementXmlModels.Add(requirementXmlModel);
				}
			}
		}
	}

	public override bool IsSatisfied()
	{
		for (int i = 0, n = requirementXmlModels.Count; i < n; i++)
		{
			if (!requirementXmlModels[i].IsSatisfied())
			{
				return false;
			}
		}
		return true;
	}
}

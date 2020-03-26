using System.Collections.Generic;
using System.Xml.Linq;

public class PathXmlModel : XmlModel
{
	public List<PointXmlModel> pointXmlModels;

	public PathXmlModel()
	{
	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);
		pointXmlModels = ParseModelsFromChildElement<PointXmlModel>(element, "point");
	}
}
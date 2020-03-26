using System.Xml.Linq;

public class PointXmlModel : XmlModel
{
	public Vector3XmlModel positionXmlModel;

	public override void Initialize(XElement element)
	{
		base.Initialize(element);
		positionXmlModel = ParseModelFromChildElement<Vector3XmlModel>(element, "localPosition");
	}
}
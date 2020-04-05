using System.Xml.Linq;

public abstract class RequirementXmlModel : XmlModel
{
	public RequirementXmlModel()
	{

	}

	public override void Initialize(XElement element)
	{
		base.Initialize(element);
	}

	public abstract bool IsSatisfied();
}

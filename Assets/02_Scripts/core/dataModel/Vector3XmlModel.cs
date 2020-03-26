using System.Xml.Linq;

public class Vector3XmlModel : XmlModel
{
    public float x;
    public float y;
    public float z;

    public Vector3XmlModel()
    {
    }

    public override void Initialize(XElement element)
    {
        base.Initialize(element);

        x = ParseFloatAttribute(element, "x");
        y = ParseFloatAttribute(element, "y");
        z = ParseFloatAttribute(element, "z");
    }
}
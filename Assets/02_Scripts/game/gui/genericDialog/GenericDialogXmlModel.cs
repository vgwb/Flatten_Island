using System.Xml.Linq;

public class GenericDialogXmlModel : XmlModel
{
	public string title;
	public string message;
	public int buttons;

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		title = ParseStringAttribute(element, "title");
		message = ParseStringAttribute(element, "message");
		buttons = ParseIntAttribute(element, "buttons", 1);
	}
}

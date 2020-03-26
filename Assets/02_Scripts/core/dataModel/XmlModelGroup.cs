using System.Xml.Linq;
using UnityEngine;

public class XmlModelGroup : XmlModel
{
	public const string ELEMENT_NAME = "group";

	override public void Initialize(XElement element)
	{
		base.Initialize(element);
		
		foreach (XElement childElement in element.Elements())
		{
			if (childElement != null)
			{
				XmlModel xmlModel = XmlModelTypeMappings.instance.CreateInstanceFromName(childElement.Name.ToString()) as XmlModel;
				if (xmlModel != null)
				{
					xmlModel.Initialize(childElement);
					XmlModelManager.instance.RegisterModel(xmlModel);
				}
				else
				{
					Debug.LogError("Xml element " + childElement.Name.ToString() + " does not have a mapping in code.");
				}
			}
		}
	}
}


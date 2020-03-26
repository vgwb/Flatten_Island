using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;

public class XmlModel
{
	public int id;

	virtual public string name
	{
		get; protected set;
	}

	virtual public void Initialize(XElement element)
	{
		id = ParseIntAttribute(element, "id");
		name = ParseStringAttribute(element, "name");
	}

	protected static Predicate<T> FindById<T>(int id) where T: XmlModel
	{
		return delegate(T model)
		{
			return model.id == id;
		};
	}

	public static int ParseIntAttribute(XElement element, string attributeName, int defaultValue = 0)
	{
		string attributeValue = (string)element.Attribute(attributeName);
		if (attributeValue != null)
		{
			int intValue;
			if (int.TryParse(attributeValue, out intValue))
			{
				return intValue;
			}	
		}
		return defaultValue;
	}
	
	public static long ParseLongAttribute(XElement element, string attributeName, long defaultValue = 0)
	{
		string attributeValue = (string)element.Attribute(attributeName);
		if (attributeValue != null)
		{
			long longValue;
			if (long.TryParse(attributeValue, out longValue))
			{
				return longValue;
			}	
		}
		return defaultValue;
	}
	
	public static float ParseFloatAttribute(XElement element, string attributeName, float defaultValue = 0)
	{
		string attributeValue = (string)element.Attribute(attributeName);
		if (attributeValue != null)
		{
			return float.Parse(attributeValue, CultureInfo.InvariantCulture);
		}
		return defaultValue;
	}
	
	public static string ParseStringAttribute(XElement element, string attributeName, string defaultValue = null)
	{
		string attributeValue = (string)element.Attribute(attributeName);
		if (attributeValue != null)
		{
			return attributeValue;
		}
		return defaultValue;
	}
	
	public static bool ParseBoolAttribute(XElement element, string attributeName, bool defaultValue = false)
	{
		string attributeValue = (string)element.Attribute(attributeName);
		if (attributeValue != null)
		{
			bool val;
			if (bool.TryParse(attributeValue, out val))
			{
				return val;
			}
		}
		return defaultValue;
	}
	
	public static int ParseIntFromChildElement(XElement element, string childElementName, int defaultValue = 0)
	{
		XElement childElement = element.Element(childElementName);
		if (childElement != null)
		{
			return int.Parse(childElement.Value);
		}
		return defaultValue;
	}
	
	public static float ParseFloatFromChildElement(XElement element, string childElementName, float defaultValue = 0)
	{
		XElement childElement = element.Element(childElementName);
		if (childElement != null)
		{
			return float.Parse(childElement.Value, CultureInfo.InvariantCulture);
		}
		return defaultValue;
	}

	public static bool ParseBoolFromChildElement(XElement element, string childElementName, bool defaultValue = false)
	{
		XElement childElement = element.Element(childElementName);
		if (childElement != null)
		{
			return bool.Parse(childElement.Value);
		}
		return defaultValue;
	}

	public static string ParseStringFromChildElement(XElement element, string childElementName, string defaultValue = null)
	{
		XElement childElement = element.Element(childElementName);
		if (childElement != null)
		{
			return childElement.Value;
		}
		return defaultValue;
	}
	
	public static string[] ParseStringsFromChildElement(XElement element, string childElementName)
	{
		List<string> strings = new List<string>();
		foreach (XElement childElement in element.Elements(childElementName))
		{
			strings.Add(childElement.Value);
		}
		
		return strings.ToArray();
	}

	public static T ParseEnumAttribute<T>(XElement element, string attributeName, bool ignoreCase = false, T defaultValue = default(T))
	{
		string stringValue = ParseStringAttribute(element, attributeName);
		
		if (stringValue == null) { return defaultValue; }
		
		T value = (T)Enum.Parse(typeof(T), stringValue, ignoreCase);
		return value;
	}

	public DateTime ParseDateAttribute(XElement element, string attributeName, DateTime defaultDate = default(DateTime))
	{
		string attributeValue = (string)element.Attribute(attributeName);
		if (attributeValue != null)
		{
			try
			{
				DateTime dateTime = XmlConvert.ToDateTime(attributeValue, XmlDateTimeSerializationMode.Unspecified);
				return dateTime;
			}
			catch
			{}
		}
		return defaultDate;
	}
	
	public XElement GetFirstChildElement(XElement element)
	{
		XElement childElement = element.FirstNode as XElement;
		if (childElement != null)
		{
			return childElement;
		}
		return null;
	}
	
	public static T ParseModelFromChildElement<T>(XElement element, string childElementName) where T : XmlModel, new()
	{
		XElement childElement = element.Element(childElementName);
		if (childElement != null)
		{
			T model = new T();
			model.Initialize(childElement);
			return model;
		}
		return null;
	}
	
	public static List<T> ParseModelsFromChildElement<T>(XElement element, string childElementName) where T : XmlModel, new()
	{
		List<T> models = new List<T>();
		if (element != null)
		{
			foreach (XElement childElement in element.Elements(childElementName))
			{
				if (childElement != null)
				{
					T model = new T();
					model.Initialize(childElement);
					models.Add(model);
				}
			}
		}
		
		return models;
	}

	/*
	public System.Object CreateInstanceFromName()
	{
		try 
		{
			return Activator.CreateInstance(this.GetType());
		} 
		catch 
		{
			//[FC] Raise an error + assert here when we have our console
			return null;
		}
	}
	*/
}
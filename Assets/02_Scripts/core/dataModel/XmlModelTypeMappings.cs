using System;
using System.Collections.Generic;
using UnityEngine;

public class XmlModelTypeMappings : Singleton
{
	public static XmlModelTypeMappings instance
	{
		get
		{
			return GetInstance<XmlModelTypeMappings>();
		}
	}

	public Dictionary<string, Type> mappings = new Dictionary<string, Type>();
	
	public void InitializeCoreMappings()
	{
		mappings.Add(XmlModelGroup.ELEMENT_NAME, typeof(XmlModelGroup));
	}

	public void AddMapping(string xmlName,Type xmlModelType)
	{
		if (!mappings.ContainsKey(xmlName))
		{
			mappings.Add(xmlName, xmlModelType);
		}
	}

	public void RemoveMapping(string xmlName)
	{
		mappings.Remove(xmlName);
	}

	public object CreateInstanceFromName(string xmlName)
	{
		if (mappings.ContainsKey(xmlName))
		{
			return Activator.CreateInstance(instance.mappings[xmlName]);
		}
		else
		{
			Debug.LogError(xmlName + " is not a recognized name for a Model");
		}

		return null;
	}
}
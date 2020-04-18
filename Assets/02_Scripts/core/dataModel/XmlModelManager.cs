using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class XmlModelManager : Singleton
{
	public static XmlModelManager instance
	{
		get
		{
			return GetInstance<XmlModelManager>();
		}
	}
	
	private Dictionary<int, XmlModel> xmlModels = new Dictionary<int, XmlModel>();
	
	public void LoadXml(string xmlText, string name = null)
	{
		XDocument xmlDocument = XDocument.Parse(xmlText);

		XmlModelGroup topGroup = new XmlModelGroup();
		topGroup.Initialize(xmlDocument.Root);
	}
	
	public void RegisterModel(XmlModel xmlModel)
	{
		XmlModel duplicate = FindModel(xmlModel.id);
		if (duplicate != null)
		{
			Debug.LogError("XmlModel - Duplicated id:" + duplicate.id);
		}
		
		xmlModels.Add(xmlModel.id, xmlModel);
	}

	public bool RemoveModel(int id)
	{
		return xmlModels.Remove(id);
	}

	public void RegisterModels(XmlModel[] models)
	{
		foreach (XmlModel model in models)
		{
			RegisterModel(model);
		}
	}

	public void UnregisterAllModels()
	{
		xmlModels.Clear();
	}

	public object CreateInstanceFromId(int id)
	{
		XmlModel xmlModel = FindModel(id);

		if (xmlModel != null) 
		{
			try 
			{
				return Activator.CreateInstance(xmlModel.GetType());
			} 
			catch(Exception exception)
			{
				Debug.LogError("XmlModelManager: Can't create instance of type:" + xmlModel.GetType() + " - Exception:" + exception.Message);
			}
		}

		return null;
	}
	
	public XmlModel FindModel(int id)
	{
		XmlModel xmlModel;
		if (xmlModels.TryGetValue(id, out xmlModel))
		{
			return xmlModel;
		}
		return null;
	}
	
	public T FindModel<T>(int id) where T : XmlModel
	{
		return FindModel(id) as T;
	}
	
	public T FindModel<T>() where T : XmlModel
	{
		foreach (var pair in xmlModels)
		{
			T value = pair.Value as T;
			if (value != null)
			{
				return value;
			}
		}
		return null;
	}
	
	public List<T> FindModels<T>() where T : XmlModel
	{
		List<T> matchingModels = new List<T>();
		foreach (var pair in xmlModels)
		{
			T value = pair.Value as T;
			if (value != null)
			{
				matchingModels.Add(value);
			}
		}
		return matchingModels;
	}
	
	public delegate bool MatchModel<T>(T model);
	public List<T> FindModels<T>(MatchModel<T> matchModel) where T : XmlModel
	{
		List<T> matchingModels = new List<T>();
		foreach (var pair in xmlModels)
		{
			T value = pair.Value as T;
			if (value != null && matchModel(value))
			{
				matchingModels.Add(value);
			}
		}
		return matchingModels;
	}
}
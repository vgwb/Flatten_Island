using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton
{
	private static Dictionary<Type, Singleton> instances = new Dictionary<Type, Singleton>();

    public Singleton()
	{
		RegisterInstance();
	}
	
	private void RegisterInstance()
	{
		Type type = GetType();

		//Debug.Log("Registered Singleton:" + type.Name);

		instances.Add(type, this);
	}

    protected static T GetInstance<T>() where T : Singleton, new()
	{
		Singleton instance;
		instances.TryGetValue(typeof(T), out instance);
		
		if (instance == null)
		{
			instance = new T();
		}
		
		return instance as T;
	}
}

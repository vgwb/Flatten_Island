using System;
using System.Collections.Generic;

public class MonoSingleton : BaseBehaviour
{
	private static Dictionary<Type, MonoSingleton> instances = new Dictionary<Type, MonoSingleton>();

	private void Awake()
	{
		RegisterInstance();
		OnMonoSingletonAwake();
	}

	private void Start()
	{
		OnMonoSingletonStart();
	}

	private void Update()
	{
		OnMonoSingletonUpdate();
	}

	private void RegisterInstance()
	{
		Type type = GetType();

		if (instances.ContainsKey(type))
		{
			instances.Remove(type);
		}

		instances.Add(type, this);
	}

	protected virtual void OnMonoSingletonAwake() { }
	protected virtual void OnMonoSingletonStart() { }
	protected virtual void OnMonoSingletonUpdate() { }

	private void OnDestroy()
	{
		OnMonoSingletonDestroyed();
		RemoveInstance();
	}

	private void RemoveInstance()
	{
		Type type = GetType();
		instances.Remove(type);
	}

	protected virtual void OnMonoSingletonDestroyed() { }

	protected static T GetInstance<T>() where T : MonoSingleton
	{
		MonoSingleton instance;
		instances.TryGetValue(typeof(T), out instance);
		return instance as T;
	}
}
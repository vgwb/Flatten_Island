using UnityEngine;

public class EventObject : ScriptableObject
{
	public static T GetInstance<T>(EventObject eventObject) where T : EventObject
	{
		T instance = eventObject as T;
		if (instance == null)
		{
			instance = CreateInstance<T>();
			instance.name = typeof(T).Name;
			if (!eventObject.name.Equals(instance.name))
			{
				return null;
			}
		}

		return instance;
	}

}
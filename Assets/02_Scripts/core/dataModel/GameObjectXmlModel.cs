using System.Xml.Linq;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectXmlModel : XmlModel
{
	public readonly int DEFAULT_NUMBER_OF_PRELOADED_INSTANCES = 1;

	public string prefab;
	public int preloaded;

    public GameObjectXmlModel()
    {
    }

    override public void Initialize(XElement element)
    {
        base.Initialize(element);
        prefab = ParseStringAttribute(element, "prefab");
		preloaded = ParseIntAttribute(element, "preloaded", DEFAULT_NUMBER_OF_PRELOADED_INSTANCES);
	}

	public virtual GameObject CreateGameObject()
    {
		return GameObjectFactory.instance.InstantiateGameObject(prefab, null);
	}

	public virtual void ReleaseGameObject(GameObject gameObject)
	{
		GameObjectFactory.instance.ReleaseGameObject(gameObject, prefab);
	}

	public void PreloadInstances()
	{
		if (preloaded > 0)
		{
			List<GameObject> gameObjects = new List<GameObject>(preloaded);

			for (int i = 0; i < preloaded; ++i)
			{
				gameObjects.Add(CreateGameObject());
			}

			foreach (GameObject gameObject in gameObjects)
			{
				ReleaseGameObject(gameObject);
			}

			gameObjects.Clear();
			gameObjects = null;
		}

	}
}
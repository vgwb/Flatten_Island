using UnityEngine;
using System.Collections.Generic;

public class GameObjectFactory : Singleton
{
    public static GameObjectFactory instance
    {
        get
        {
            return GetInstance<GameObjectFactory>();
        }
    }

	private static GameObject deactivePoolsRootFolder = null;

	private Dictionary<string, GameObjectPool> poolsByName;

    public GameObjectFactory()
    {
        poolsByName = new Dictionary<string, GameObjectPool>();
    }

	public void CreatePoolRootFolderObject(Transform parent)
	{
		if (deactivePoolsRootFolder == null)
		{
			deactivePoolsRootFolder = new GameObject();
			deactivePoolsRootFolder.name = "_GameObjectPools";
			deactivePoolsRootFolder.transform.parent = parent;
		}
	}

	public GameObject InstantiateGameObject(string prefab, Transform spawnTransform)
	{
		return InstantiateGameObject(prefab, spawnTransform, false);
	}

    public GameObject InstantiateGameObject(string prefab, Transform spawnTransform, bool attachToSpawnTransformAsChild)
    {
        GameObjectPool gameObjectPool;
        poolsByName.TryGetValue(prefab, out gameObjectPool);

        if (gameObjectPool == null)
        {
            gameObjectPool = new GameObjectPool(prefab, deactivePoolsRootFolder);
            poolsByName.Add(prefab, gameObjectPool);
        }

        GameObject go = gameObjectPool.Instantiate() as GameObject;

		if (go != null)
		{
			BaseBehaviour baseBehaviour = go.GetComponent<BaseBehaviour>();
			IPoolable iPoolable = baseBehaviour as IPoolable;
			if (iPoolable != null)
			{
				iPoolable.OnInstantiatedFromPool();
			}

			if (spawnTransform != null)
			{
				if (attachToSpawnTransformAsChild)
				{
					GameObjectUtils.instance.AttachGameObjectToParent(go.transform, spawnTransform, true, true, true);
				}
				else
				{
					go.transform.position = spawnTransform.position;
				}
			}
		}

		return go;
    }

    public void ReleaseGameObject(GameObject gameObject, string prefab)
    {
        GameObjectPool gameObjectPool;
        poolsByName.TryGetValue(prefab, out gameObjectPool);

        if (gameObjectPool != null)
        {
			BaseBehaviour baseBehaviour = gameObject.GetComponent<BaseBehaviour>();
			IPoolable iPoolable = baseBehaviour as IPoolable;
			if (iPoolable != null)
			{
				iPoolable.OnReleasedToPool();
			}

			gameObjectPool.Release(gameObject);
		}
	}
}
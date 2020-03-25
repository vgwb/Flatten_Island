using UnityEngine;
using System.Collections.Generic;

public class GameObjectPool
{
    private string prefabName;
    private GameObject prefabGameObject;
    private Stack<GameObject> pool;
	private GameObject deactiveFolder;
	private GameObject deactivePoolsRootFolder = null;

    public GameObjectPool(string prefabName, GameObject deactivePoolsRootFolder)
    {
        pool = new Stack<GameObject>();
        this.prefabName = prefabName;
        prefabGameObject = null;
		this.deactivePoolsRootFolder = deactivePoolsRootFolder;
		CreateDeactivePoolFolderObject();
	}

	private void CreateDeactivePoolFolderObject()
	{
		deactiveFolder = new GameObject();
		deactiveFolder.name = "_" + prefabName;
		deactiveFolder.transform.SetParent(deactivePoolsRootFolder.transform);
	}

	public void UnloadAll()
    {
        prefabGameObject = null;
        DestroyAll();
        Resources.UnloadUnusedAssets();
    }

    private void DestroyAll()
    {
        while (pool.Count > 0)
        {
            GameObject gameObject = pool.Pop();
            Object.Destroy(gameObject);
        }
    }

    public GameObject Instantiate()
    {
		GameObject gameObject = null;

        if (pool.Count == 0)
        {
			//Debug.Log(prefabName + " pool ran out of instances! Creating extra copy");
            gameObject = TryCreateNew();
        }
        else
        {
			gameObject = pool.Pop();
		}

		gameObject.name = prefabName + "_" + pool.Count;
		gameObject.transform.SetParent(null);
		return gameObject;
    }

    private GameObject TryCreateNew()
    {
        GameObject prefab = GetPrefabGameObject();
        if (prefab != null)
        {
			GameObject gameObject = Object.Instantiate(prefab);
			gameObject.SetActive(false);
            return gameObject;
        }

        return null;
    }

    private GameObject GetPrefabGameObject()
    {
        if (prefabGameObject == null)
        {
			prefabGameObject = Resources.Load(prefabName) as GameObject;
            if (prefabGameObject == null)
            {
				Debug.LogError(prefabName + " not found in Resources folder");
            }
        }
        return prefabGameObject;
    }

    public void Release(GameObject gameObject)
    {
        gameObject.SetActive(false);
        gameObject.transform.SetParent(deactiveFolder.transform);
        pool.Push(gameObject);
    }
}


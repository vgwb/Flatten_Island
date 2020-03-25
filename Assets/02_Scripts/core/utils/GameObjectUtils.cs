using UnityEngine;
using System.Collections.Generic;

public class GameObjectUtils : Singleton
{
    public static GameObjectUtils instance
    {
        get
        {
            return GetInstance<GameObjectUtils>();
        }
    }

    public void ResetTransform(Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void SetLocalPosition(Transform transform, float x, float y, float z)
    {
        Vector3 newLocalPosition = new Vector3(x, y, z);
        transform.localPosition = newLocalPosition;
    }

	public void SetLocalRotation(Transform transform, float x, float y, float z)
	{
		Vector3 newLocalRotation = new Vector3(x, y, z);
		transform.localEulerAngles = newLocalRotation; 
	}

	public void SetLocalScale(Transform transform, float x, float y, float z)
    {
        Vector3 newLocalScale = new Vector3(x, y, z);
        transform.localScale = newLocalScale;
    }

    public GameObject CreateGameObject(string name, Transform parent)
    {
        GameObject gameObject = new GameObject();
        gameObject.name = name;
        Transform transform = gameObject.transform;
        transform.parent = parent;
        ResetTransform(transform);
        return gameObject;
    }

    public void AttachGameObjectToParent(Transform transform, Transform parent, bool retainLocalPosition, bool retainLocalRotation, bool retainLocalScale)
    {
		Vector3	localPosition = transform.localPosition;
		Quaternion localRotation = transform.localRotation;
		Vector3	localScale = transform.localScale;

        transform.parent = parent;

		// restore local position/scale/rotation since they get changed to compensate for the parent's local position/scale/rotation
		if (retainLocalPosition)
		{
			transform.localPosition = localPosition;
		}

		if (retainLocalRotation)
		{
			transform.localRotation = localRotation;
		}

		if (retainLocalScale)
		{
			transform.localScale = localScale;
		}
	}

    public void DetachGameObjectFromParent(Transform transform)
    {
        transform.parent = null;
    }


    public void DestroyAllChildren(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void DestroyAllChildrenImmediate(Transform transform)
    {
        int count = transform.childCount;
        if (count > 0)
        {
            List<GameObject> children = new List<GameObject>(count);
            foreach (Transform childTransform in transform)
            {
                children.Add(childTransform.gameObject);
            }

            for (int i = 0; i < children.Count; i++)
            {
                GameObject.DestroyImmediate(children[i]);
            }
        }
    }

	public bool IsTransformFlipped(Transform transform)
	{
		return transform.localScale.z == -1.0f;
	}

	public void RotateToDirection(GameObject gameObject, Vector3 fromNormalizedDirection, Vector3 toNormalizedDirection)
	{
		//[FC] This function should be generic and get the UP vector as well. At the moment is considering UP is y positive axis

		float angle = Vector3.Angle(fromNormalizedDirection, toNormalizedDirection);
		float signedAngle = angle * Mathf.Sign(Vector3.Cross(fromNormalizedDirection, toNormalizedDirection).y);

		if (signedAngle >= 0)
		{
			//clock rotation
			gameObject.transform.Rotate(Vector3.up, angle);
		}
		else
		{
			//counter-clock rotation
			gameObject.transform.Rotate(Vector3.down, angle);
		}
	}

}
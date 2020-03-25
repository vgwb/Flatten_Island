using UnityEngine;
using Messages;

public class InputManager : MonoSingleton
{
	private const int MAX_NUMBER_OF_COLLIDERS_TO_DETECT = 7;

	public Camera inputCamera;

	public static InputManager instance
	{
		get
		{
			return GetInstance<InputManager>();
		}
	}

	private Collider2D[] FindOverlapping2DCollidersOnScreenPoint(Vector2 screenPoint)
	{
		Vector2 v = inputCamera.ScreenToWorldPoint(screenPoint);
		Collider2D[] colliders = new Collider2D[MAX_NUMBER_OF_COLLIDERS_TO_DETECT];
		Physics2D.OverlapPointNonAlloc(v, colliders);
		return colliders;
	}

	private BaseBehaviour FindFirstOverlappingViewOnScreenPoint(Vector2 screenPoint)
	{
		Collider2D[] overlappingColliders = FindOverlapping2DCollidersOnScreenPoint(screenPoint);
		foreach (Collider2D collider in overlappingColliders)
		{
			if (collider != null)
			{
				BaseBehaviour firstCollidedView = collider.GetComponent<BaseBehaviour>();
				if (firstCollidedView == null)
				{
					firstCollidedView = collider.GetComponentInParent<BaseBehaviour>();
				}

				if (firstCollidedView != null)
				{
					return firstCollidedView;
				}
			}
		}
		return null;
	}

	private bool IsOnUiObject(Vector2 screenPosition)
	{
		Ray inputRay = inputCamera.ScreenPointToRay(screenPosition);
		RaycastHit hit;
		return Physics.Raycast(inputRay.origin, inputRay.direction, out hit, Mathf.Infinity, ~LayerMask.NameToLayer("UI"));
	}
}

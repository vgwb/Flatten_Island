using UnityEngine;
using System;

public class ScaleObject 
{
	private Transform targetTransform;
	public Vector3 fromScale { get; private set; }
	public Vector3 toScale { get; private set; }
	private float speed;
	private float acceleration;
	public Action onCompleted;

	public bool isScaling { get; private set; }

	public ScaleObject(Transform targetTransform, Vector3 fromScale, Vector3 toScale, float speed, float acceleration)
	{
		this.targetTransform = targetTransform;
		this.speed = speed;
		this.acceleration = acceleration;
		this.toScale = toScale;
		this.fromScale = fromScale;
		isScaling = false;
	}
	public void SetOnCompleted(Action onCompleted)
	{
		this.onCompleted = onCompleted;
	}

	public void Start()
	{
		isScaling = true;
		targetTransform.localScale = fromScale;
	}

	public void Update(float deltaTime)
	{
		if (isScaling)
		{
			if (targetTransform.localScale.Equals(toScale))
			{
				Complete();
			}
			else
			{
				speed += acceleration;
				float step = speed * deltaTime;
				targetTransform.localScale = Vector3.MoveTowards(targetTransform.localScale, toScale, step);
				if (IsCompleted())
				{
					Complete();
				}
			}
		}
	}

	public void Abort()
	{
		isScaling = false;
	}

	private void Complete()
	{
		isScaling = false;
		if (onCompleted != null)
		{
			onCompleted();
		}
	}

	private bool IsCompleted()
	{
		if (targetTransform.localScale.Equals(toScale))
		{
			return true;
		}

		if (fromScale.x <= toScale.x && fromScale.y <= toScale.y && fromScale.z <= toScale.z)
		{
			if (targetTransform.localScale.x >= toScale.x && targetTransform.localScale.y >= toScale.y && targetTransform.localScale.z >= toScale.z)
			{
				return true;
			}
		}

		if (fromScale.x >= toScale.x && fromScale.y >= toScale.y && fromScale.z >= toScale.z)
		{
			if (targetTransform.localScale.x <= toScale.x && targetTransform.localScale.y <= toScale.y && targetTransform.localScale.z <= toScale.z)
			{
				return true;
			}
		}

		return false;
	}
}

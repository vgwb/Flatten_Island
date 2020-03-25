using System;
using UnityEngine;

public class MoveObject
{
	private Transform targetTransform;
	public Vector3? destinationPosition { get; private set; }
	private float speed;
	private bool isLocal;
	private float distanceFromDestination;
	public Action onCompleted;

	public bool isMoving { get; private set; }

	public MoveObject(Transform targetTransform, float speed, bool isLocal = false)
	{
		this.targetTransform = targetTransform;
		this.speed = speed;
		this.isLocal = isLocal;
		distanceFromDestination = 0.0f;
		destinationPosition = null;
		isMoving = false;
	}

	public MoveObject(Transform targetTransform, Vector3 destinationPosition, float speed, bool isLocal = false)
	{
		this.targetTransform = targetTransform;
		this.speed = speed;
		this.isLocal = isLocal;
		distanceFromDestination = 0.0f;
		SetDestination(destinationPosition);
		isMoving = false;
	}

	public MoveObject(Transform targetTransform, float speed, float distanceFromDestination, bool isLocal = false)
	{
		this.targetTransform = targetTransform;
		this.speed = speed;
		this.isLocal = isLocal;
		this.distanceFromDestination = distanceFromDestination;
		destinationPosition = null;
		isMoving = false;
	}

	public MoveObject(Transform targetTransform, Vector3 destinationPosition, float speed, float distanceFromDestination, bool isLocal = false) : this(targetTransform, destinationPosition, speed, isLocal)
	{
		this.distanceFromDestination = distanceFromDestination;
	}

	public void SetOnCompleted(Action onCompleted)
	{
		this.onCompleted = onCompleted;
	}

	public void SetDestination(Vector3? destinationPosition)
	{
		this.destinationPosition = destinationPosition;
	}

	public void Start()
	{
		isMoving = true;
	}

	public void Update(float deltaTime)
	{
		if (isMoving)
		{
			if (isLocal)
			{
				if (targetTransform.localPosition.Equals(destinationPosition))
				{
					Complete();
				}
				else
				{
					float step = speed * deltaTime;
					targetTransform.localPosition = Vector3.MoveTowards(targetTransform.localPosition, destinationPosition.Value, step);

					if (distanceFromDestination > 0.0f)
					{
						float distance = Vector3.Distance(targetTransform.localPosition, destinationPosition.Value);
						if (distance <= distanceFromDestination)
						{
							Complete();
						}
					}
					else
					{
						if (targetTransform.localPosition.Equals(destinationPosition))
						{
							Complete();
						}
					}
				}
			}
			else
			{
				float distance = Vector3.Distance(targetTransform.position, destinationPosition.Value);
				if (distance <= MathUtils.EPSILON)
				{
					Complete();
				}
				else
				{
					float step = speed * deltaTime;
					targetTransform.position = Vector3.MoveTowards(targetTransform.position, destinationPosition.Value, step);

					if (distanceFromDestination > 0.0f)
					{
						distance = Vector3.Distance(targetTransform.position, destinationPosition.Value);
						if (distance <= distanceFromDestination)
						{
							Complete();
						}
					}
					else
					{
						distance = Vector3.Distance(targetTransform.position, destinationPosition.Value);
						if (distance <= MathUtils.EPSILON)
						{
							Complete();
						}
					}
				}
			}
		}
	}

	public void Abort()
	{
		isMoving = false;
	}

	private void Complete()
	{
		isMoving = false;
		onCompleted?.Invoke();
	}
}

using UnityEngine;

public class ScrollUpIngredient : Ingredient
{
	private const int DISTANCE_TO_COVER = 10000;

	public Transform transformToMove;

	public float speed;

	private MoveObject moveObject;

	public override void Prepare()
	{
		base.Prepare();

		Vector3 destinationPosition = transformToMove.position;
		destinationPosition.y += DISTANCE_TO_COVER;

		moveObject = new MoveObject(transformToMove, destinationPosition, speed);
		moveObject.SetOnCompleted(OnMoveCompleted);
		moveObject.Start();
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			moveObject.Update(deltaTime);
		}

		return status;
	}

	private void OnMoveCompleted()
	{
		status = CookbookStatus.Success;
	}
}

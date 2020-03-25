using UnityEngine;

public class ScaleObjectIngredient : Ingredient
{
	public Transform targetTransform;
	public Vector3 fromScale;
	public Vector3 toScale;
	public float speed;
	public float acceleration = 1f;

	private ScaleObject scaleObject;

	public override void Prepare()
	{
		base.Prepare();
		scaleObject = new ScaleObject(targetTransform, fromScale, toScale, speed, acceleration);
		scaleObject.SetOnCompleted(OnScaleCompleted);
		scaleObject.Start();
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			scaleObject.Update(deltaTime);
		}

		return status;
	}

	private void OnScaleCompleted()
	{
		status = CookbookStatus.Success;
	}
}

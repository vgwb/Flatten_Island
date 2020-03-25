using UnityEngine;

public class EnableColliderIngredient : Ingredient
{
	public Collider targetCollider;
	public bool toEnable;

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			targetCollider.enabled = toEnable;
			status = CookbookStatus.Success;
		}

		return status;
	}
}
using UnityEngine;

public class ActivateGameObjectIngredient : Ingredient
{
	public GameObject targetGameObject;
	public bool active;

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			targetGameObject.SetActive(active);
			status = CookbookStatus.Success;
		}

		return status;
	}
}

using UnityEngine;

public class CookForeignRecipeIngredient : Ingredient
{
	public Chef chef;
	public Recipe recipeToCook;

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			if (chef.CanCook())
			{
				chef.Cook(recipeToCook, null, null, false);
				status = CookbookStatus.Success;
			}
			else
			{
				Debug.LogError("CookRecipeIngredient - chef:" + chef.gameObject.name + " can't cook recipe " + recipeToCook.gameObject.name);
				status = CookbookStatus.Failure;
			}
		}

		return status;
	}
}

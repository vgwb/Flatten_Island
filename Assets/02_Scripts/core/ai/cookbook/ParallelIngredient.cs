using System.Collections.Generic;
using UnityEngine;

public class ParallelIngredient : Ingredient
{
	private List<Ingredient> ingredients;
	private List<CookbookStatus> ingredientsStatus;


	private void Awake()
	{
		ingredients = new List<Ingredient>();
		ingredientsStatus = new List<CookbookStatus>();

		foreach (Transform child in transform)
		{
			Ingredient ingredient = child.gameObject.GetComponent<Ingredient>();
			if (ingredient != null && ingredient.gameObject.activeSelf)
			{
				ingredient.SetRecipe(recipe);
				ingredients.Add(ingredient);
			}
		}
	}

	public override void Prepare()
	{
		base.Prepare();

		ingredientsStatus.Clear();

		foreach (Ingredient ingredient in ingredients)
		{
			ingredient.Prepare();
			ingredientsStatus.Add(CookbookStatus.Running);
		}
	}

	public override CookbookStatus Use(float deltaTime)
	{
		if (status == CookbookStatus.Running)
		{
			for (int i = 0; i < ingredients.Count; i++)
			{
				ingredientsStatus[i] = ingredients[i].Use(deltaTime);
			}

			int successCount = 0;
			int failureCount = 0;

			foreach (CookbookStatus ingredientStatus in ingredientsStatus)
			{
				if (ingredientStatus == CookbookStatus.Success)
				{
					successCount++;
				}
				else if (ingredientStatus == CookbookStatus.Failure)
				{
					failureCount++;
				}
			}

			if (successCount == ingredients.Count)
			{
				status = CookbookStatus.Success;
				return status;
			}
			else if (failureCount > 0)
			{
				status = CookbookStatus.Failure;
				return status;
			}
		}

		return status;
	}
}

using System.Collections.Generic;
using UnityEngine;

public class SequenceIngredient : Ingredient
{
	private List<Ingredient> ingredients;
	private RemovableQueue<Ingredient> ingredientsQueue = null;

	private void Awake()
	{
		ingredients = new List<Ingredient>();
		ingredientsQueue = new RemovableQueue<Ingredient>();

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

		if (ingredients.Count > 0)
		{
			ingredientsQueue.Clear();
			foreach (Ingredient ingredient in ingredients)
			{
				ingredientsQueue.Enqueue(ingredient);
			}

			Ingredient currentIngredient = ingredientsQueue.Peek();
			currentIngredient.Prepare();
		}
	}

	public override CookbookStatus Use(float deltaTime)
	{
		if (status == CookbookStatus.Running)
		{
			if (ingredientsQueue.Count > 0)
			{
				Ingredient currentIngredient = ingredientsQueue.Peek();
				CookbookStatus ingredientStatus = currentIngredient.Use(deltaTime);

				while (ingredientsQueue.Count > 0 && ingredientStatus == CookbookStatus.Success)
				{
					ingredientsQueue.Dequeue();

					if (ingredientsQueue.Count > 0)
					{
						currentIngredient = ingredientsQueue.Peek();
						currentIngredient.Prepare();
						ingredientStatus = currentIngredient.Use(deltaTime);
					}
				}

				if (ingredientStatus == CookbookStatus.Failure)
				{
					status = CookbookStatus.Failure;
					return status;
				}
				else if (ingredientStatus == CookbookStatus.Running)
				{
					status = CookbookStatus.Running;
					return status;
				}
			}

			status = CookbookStatus.Success;
		}

		return status;
	}
}

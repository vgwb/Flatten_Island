using System.Collections.Generic;
using UnityEngine;

public class CountedLoopIngredient : Ingredient
{
	public int times;
	private List<Ingredient> ingredients;
	private int currentIndex;

	private int timesCount;

	private void Awake()
	{
		ingredients = new List<Ingredient>();

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

		timesCount = 0;
		if (ingredients.Count > 0)
		{
			currentIndex = 0;
			Ingredient currentIngredient = ingredients[currentIndex];
			currentIngredient.Prepare();
		}
	}

	public override CookbookStatus Use(float deltaTime)
	{
		if (status == CookbookStatus.Running)
		{
			if (ingredients.Count > 0)
			{
				Ingredient currentIngredient = ingredients[currentIndex];
				CookbookStatus ingredientStatus = currentIngredient.Use(deltaTime);

				while (currentIndex < ingredients.Count && ingredientStatus == CookbookStatus.Success)
				{
					currentIndex++;

					if (currentIndex < ingredients.Count)
					{
						currentIngredient = ingredients[currentIndex];
						currentIngredient.Prepare();
						ingredientStatus = currentIngredient.Use(deltaTime);
					}
				}

				if (currentIndex == ingredients.Count)
				{
					timesCount++;

					if (timesCount < times)
					{
						currentIndex = 0;
						currentIngredient = ingredients[currentIndex];
						currentIngredient.Prepare();
					}
					else
					{
						status = CookbookStatus.Success;
					}
				}
			}
		}

		return status;
	}
}

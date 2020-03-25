using System;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
	public Action onRecipeAborted;
	public Action onRecipeCompleted;

	public bool IsCooking { get; private set; }

	private List<Ingredient> ingredients;
	private RemovableQueue<Ingredient> ingredientsQueue = null;

	void Awake()
	{
		IsCooking = false;

		ingredients = new List<Ingredient>();
		ingredientsQueue = new RemovableQueue<Ingredient>();

		foreach (Transform child in transform)
		{
			Ingredient ingredient = child.gameObject.GetComponent<Ingredient>();
			if (ingredient != null && ingredient.gameObject.activeSelf)
			{
				ingredient.SetRecipe(this);
				ingredients.Add(ingredient);
			}
		}
	}

	private void Update()
	{
		gameObject.SetActive(false);
	}

	public CookbookStatus Execute(float deltaTime)
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
				IsCooking = false;
				return CookbookStatus.Failure;
			}
			else if (ingredientStatus == CookbookStatus.Running)
			{
				return CookbookStatus.Running;
			}
		}

		IsCooking = false;
		return CookbookStatus.Success;
	}

	public void StartCooking()
	{
		//UnityEngine.Debug.LogWarning("Time: " + UnityEngine.Time.realtimeSinceStartup + " - RecipeV2 - StartCooking Recipe: " + gameObject.name);

		IsCooking = true;
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

	public void Abort()
	{
		IsCooking = false;
	}

}

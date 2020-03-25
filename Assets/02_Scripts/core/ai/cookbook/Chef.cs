using System;
using UnityEngine;

public class Chef : MonoBehaviour
{
	private const int MAX_RECIPES_IN_QUEUE = 2;

	private RemovableQueue<Recipe> recipesQueue = null;

	protected virtual void Awake()
	{
		recipesQueue = new RemovableQueue<Recipe>();
	}

	private void FixedUpdate()
	{
		if (recipesQueue.Count > 0)
		{
			Recipe currentRecipe = recipesQueue.Peek();

			if (!currentRecipe.IsCooking)
			{
				currentRecipe.StartCooking();
			}

			CookbookStatus recipeStatus = currentRecipe.Execute(Time.deltaTime);

			if (recipeStatus != CookbookStatus.Running)
			{
				recipesQueue.Dequeue();
				currentRecipe.onRecipeCompleted?.Invoke();
			}
		}
	}

	public bool Cook(Recipe recipe)
	{
		return Cook(recipe, null, null, true);
	}

	public bool Cook(Recipe recipe, bool stopCurrentRecipe)
	{
		return Cook(recipe, null, null, stopCurrentRecipe);
	}

	public bool Cook(Recipe recipe, Action onRecipeCompleted)
	{
		return Cook(recipe, onRecipeCompleted, null, true);
	}

	public bool Cook(Recipe recipe, Action onRecipeCompleted, bool abortCurrentRecipe)
	{
		return Cook(recipe, onRecipeCompleted, null, abortCurrentRecipe);
	}

	public bool Cook(Recipe recipe, Action onRecipeCompleted, Action onRecipeAborted, bool abortCurrentRecipe)
	{
		if (abortCurrentRecipe)
		{
			AbortRecipe();
		}

		if (CanCook())
		{
			recipesQueue.Enqueue(recipe);
			recipe.onRecipeCompleted = onRecipeCompleted;
			recipe.onRecipeAborted = onRecipeAborted;
			return true;
		}
		else
		{
			UnityEngine.Debug.LogWarning(gameObject.name + " can't cook recipe " + recipe.gameObject.name + " because there are too many recipes in the queue");
		}

		return false;
	}

	public void StopCurrentRecipe()
	{
		if (recipesQueue.Count > 0)
		{
			Recipe currentRecipe = recipesQueue.Peek();
			currentRecipe.Abort();
			recipesQueue.Dequeue();
		}
	}

	public bool CanCook()
	{
		return recipesQueue.Count < MAX_RECIPES_IN_QUEUE;
	}

	private void AbortRecipe()
	{
		if (recipesQueue.Count > 0)
		{
			Recipe currentRecipe = recipesQueue.Peek();
			currentRecipe.Abort();
			currentRecipe.onRecipeAborted?.Invoke();
			recipesQueue.Dequeue();
		}
	}
}

using UnityEngine;
using System.Collections.Generic;

public class AnimatorEventChef : Chef
{
	private Dictionary<string, Recipe> recipes;

	protected override void Awake()
	{
		base.Awake();

		recipes = new Dictionary<string, Recipe>();

		foreach (Transform child in transform)
		{
			Recipe recipe = child.gameObject.GetComponent<Recipe>();
			if (recipe != null && recipe.gameObject.activeSelf)
			{
				recipes.Add(recipe.gameObject.name, recipe);
			}
		}
	}

	public void CookRecipeByName(string recipeName)
	{
		Recipe recipe = recipes[recipeName];
		if (recipe != null)
		{
			Cook(recipe);
		}
		else
		{
			Debug.LogError("AnimatorEventChef " + name + " doesn't have a recipe called " + recipeName);
		}
	}
}

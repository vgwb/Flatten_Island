using UnityEngine;

public class EnableRendererIngredient : Ingredient
{
	public Renderer targetRenderer;
	public bool toEnable;

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			targetRenderer.enabled = toEnable;
			status = CookbookStatus.Success;
		}

		return status;
	}
}
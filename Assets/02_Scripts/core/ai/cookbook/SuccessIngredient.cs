public class SuccessIngredient : Ingredient
{
	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			status = CookbookStatus.Success;
		}

		return status;
	}
}

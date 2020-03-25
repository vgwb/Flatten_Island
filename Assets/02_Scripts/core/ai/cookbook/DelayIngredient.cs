public class DelayIngredient : Ingredient
{
	public float delay = 0.0f;
	private float elapsedTime = 0.0f;

	public override void Prepare()
	{
		base.Prepare();
		elapsedTime = 0.0f;
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			elapsedTime += deltaTime;
			if (elapsedTime >= delay)
			{
				status = CookbookStatus.Success;
			}
			else
			{
				status = CookbookStatus.Running;
			}
		}

		return status;
	}
}

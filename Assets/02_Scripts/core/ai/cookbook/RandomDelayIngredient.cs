public class RandomDelayIngredient : Ingredient
{
	public float minSecDelay = 0.0f;
	public float maxSecDelay = 1.0f;

	private float delay = 0.0f;
	private float elapsedTime = 0.0f;

	public override void Prepare()
	{
		base.Prepare();

		delay = RandomGenerator.GetRandom(minSecDelay, maxSecDelay);
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
		}

		return status;
	}
}

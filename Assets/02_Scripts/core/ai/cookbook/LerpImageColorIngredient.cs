using UnityEngine;
using UnityEngine.UI;

public class LerpImageColorIngredient : Ingredient
{
	public Color startColor;
	public Color endColor;
	public Image image;

	public float lerpTime = 0.0f;

	private float elapsedTime = 0.0f;

	public override void Prepare()
	{
		base.Prepare();
		elapsedTime = 0.0f;

		image.color = startColor;
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			elapsedTime += deltaTime;
			if (elapsedTime >= lerpTime)
			{
				image.color = endColor;
				status = CookbookStatus.Success;
			}
			else
			{
				float t = elapsedTime / lerpTime;
				Color imageColor = image.color;
				imageColor = Color.Lerp(startColor, endColor, t);
				image.color = imageColor;

				status = CookbookStatus.Running;
			}
		}

		return status;
	}
}
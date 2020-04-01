using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeCinematicSceneIngredient : Ingredient
{
	public CinematicMenu cinematicMenu;
	public float fadeDuration;
	public Color fadeColor;

	private float fadeElapsedTime = 0.0f;
	private RawImage fadeImage;

	public override void Prepare()
	{
		base.Prepare();

		fadeElapsedTime = 0.0f;

		fadeImage = cinematicMenu.fadeImage.GetComponent<RawImage>();

		if (fadeDuration > 0.0f)
		{
			Color imageColor = fadeColor;
			imageColor.a = 1;
			fadeImage.color = imageColor;
		}
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			if (fadeDuration >= 0.0f)
			{ 
				fadeElapsedTime += deltaTime;
				if (fadeElapsedTime <= fadeDuration)
				{
					ApplyFade();
				}
				else
				{
					status = CookbookStatus.Success;
				}
			}
			else
			{
				status = CookbookStatus.Success;
			}
		}

		return status;
	}

	private void ApplyFade()
	{
		Color fadeImageColor = fadeImage.color;
		fadeImageColor.a = Mathf.Lerp(1.0f, 0.0f, fadeElapsedTime / fadeDuration);
		fadeImage.color = fadeImageColor;
	}
}

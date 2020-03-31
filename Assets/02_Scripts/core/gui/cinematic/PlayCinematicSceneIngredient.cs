using UnityEngine;
using System.Collections;

public class PlayCinematicSceneIngredient : Ingredient
{
	public GameObject cinematicScenePrefab;
	public GameObject cinematicSceneContainer;
	public float fadeInDuration;
	public Color fadeColor;

	private float elapsedTime = 0.0f;
	private GameObject cinematicScene;

	public override void Prepare()
	{
		base.Prepare();
		elapsedTime = 0.0f;

		cinematicScene = Instantiate(cinematicScenePrefab, cinematicSceneContainer.transform);
		cinematicScene.SetActive(true);
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			elapsedTime += deltaTime;
			if (elapsedTime >= fadeInDuration)
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

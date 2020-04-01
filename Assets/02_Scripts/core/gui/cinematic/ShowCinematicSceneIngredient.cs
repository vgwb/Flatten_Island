using UnityEngine;

public class ShowCinematicSceneIngredient : Ingredient
{
	public GameObject cinematicScenePrefab;
	public CinematicMenu cinematicMenu;

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			cinematicMenu.ShowScene(cinematicScenePrefab);
			status = CookbookStatus.Success;
		}

		return status;
	}
}

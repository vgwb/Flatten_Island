public class HideCurrentCinematicSceneIngredient : Ingredient
{
	public CinematicMenu cinematicMenu;

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			cinematicMenu.HideCurrentScene();
			status = CookbookStatus.Success;
		}

		return status;
	}
}

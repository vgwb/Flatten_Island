using UnityEngine;

public class PanCurrentCinematicSceneIngredient : Ingredient
{
	public CinematicMenu cinematicMenu;
	public Vector3 offset = Vector3.zero;
	public float speed;

	private MoveObject panMoveObject;
	private GameObject currentCinematicScene;


	public override void Prepare()
	{
		base.Prepare();

		if (offset != Vector3.zero)
		{
			currentCinematicScene = cinematicMenu.GetCurrentScene();
			Vector3 destination = currentCinematicScene.transform.position + offset;
			panMoveObject = new MoveObject(currentCinematicScene.transform, destination, speed);
			panMoveObject.SetOnCompleted(OnPanCompleted);
			panMoveObject.Start();
		}
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			panMoveObject.Update(deltaTime);
		}

		return status;
	}

	public void OnPanCompleted()
	{
		status = CookbookStatus.Success;
	}
}

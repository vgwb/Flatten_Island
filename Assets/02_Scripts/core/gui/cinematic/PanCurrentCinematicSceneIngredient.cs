using UnityEngine;
using Messages;

public class PanCurrentCinematicSceneIngredient : Ingredient
{
	public CinematicMenu cinematicMenu;
	public Vector3 offset = Vector3.zero;
	public float speed;
	public bool skippable = true;

	private MoveObject panMoveObject;
	private GameObject currentCinematicScene;

	public override void Prepare()
	{
		base.Prepare();

		EventMessageHandler backgroundButtonTappedEventHandler = new EventMessageHandler(this, OnBackgroundButtonTapped);
		EventMessageManager.instance.AddHandler(typeof(BackgroundButtonTappedEvent).Name, backgroundButtonTappedEventHandler);

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

		if (status == CookbookStatus.Success)
		{
			EventMessageManager.instance.RemoveHandler(typeof(BackgroundButtonTappedEvent).Name, this);
		}

		return status;
	}

	public void OnPanCompleted()
	{
		Complete();
	}

	private void OnBackgroundButtonTapped(EventMessage eventMessage)
	{
		if (skippable)
		{
			if (panMoveObject != null)
			{
				panMoveObject.Abort();
			}

			Complete();
		}
	}

	private void Complete()
	{
		status = CookbookStatus.Success;
	}
}

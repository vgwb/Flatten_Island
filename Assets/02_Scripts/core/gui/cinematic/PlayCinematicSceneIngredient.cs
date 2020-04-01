using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayCinematicSceneIngredient : Ingredient
{
	public GameObject cinematicScenePrefab;
	public CinematicMenu cinematicMenu;
	public float fadeInDuration;
	public Color fadeColor;
	public float durationTime;
	public Vector3 scenePanDirection = Vector3.zero;
	public float scenePanSpeed;

	private const float PAN_OFFSET = 800f;

	private float elapsedTime = 0.0f;
	private float fadeElapsedTime = 0.0f;
	private GameObject cinematicScene;
	private MoveObject panMoveObject;
	private RawImage fadeImage;
	private bool panEnabled;

	public override void Prepare()
	{
		base.Prepare();

		elapsedTime = 0.0f;
		fadeElapsedTime = 0.0f;
		panEnabled = false;

		fadeImage = cinematicMenu.fadeImage.GetComponent<RawImage>();

		if (fadeInDuration > 0)
		{
			Color imageColor = fadeColor;
			imageColor.a = 1;
			fadeImage.color = imageColor;
		}

		cinematicScene = Instantiate(cinematicScenePrefab, cinematicMenu.cinematicScenesContainer.transform);
		cinematicScene.SetActive(true);

		if (scenePanDirection != Vector3.zero)
		{
			panEnabled = true;
			Vector3 destination = cinematicScene.transform.position + (scenePanDirection * PAN_OFFSET);
			panMoveObject = new MoveObject(cinematicScene.transform, destination, scenePanSpeed);
			panMoveObject.Start();
		}
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			elapsedTime += deltaTime;
			if (elapsedTime >= durationTime)
			{
				cinematicScene.SetActive(false);
				status = CookbookStatus.Success;
			}
			else
			{
				if (panEnabled)
				{
					panMoveObject.Update(deltaTime);
				}

				fadeElapsedTime += deltaTime;
				if (fadeElapsedTime <= fadeInDuration && fadeInDuration > 0)
				{
					ApplyFade();
				}

				status = CookbookStatus.Running;
			}
		}

		return status;
	}

	private void ApplyFade()
	{
		Color fadeImageColor = fadeImage.color;
		fadeImageColor.a = Mathf.Lerp(1.0f, 0.0f, fadeElapsedTime / fadeInDuration);
		fadeImage.color = fadeImageColor;
	}
}

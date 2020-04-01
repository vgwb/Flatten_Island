using UnityEngine;
using System.Collections;
using Messages;

public class ShowSpeechBubbleIngredient : Ingredient
{
	public SpeechMenu speechMenu;
	public string localizationTextKey;
	public float durationTime;
	public bool skippable = true;

	private float elapsedTime = 0.0f;

	public override void Prepare()
	{
		base.Prepare();
		elapsedTime = 0.0f;
		speechMenu.ShowSpeechBubble(localizationTextKey);
	}


	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			elapsedTime += deltaTime;
			if (elapsedTime >= durationTime)
			{
				speechMenu.HideSpeechBubble();
				status = CookbookStatus.Success;
			}
		}

		return status;
	}
}

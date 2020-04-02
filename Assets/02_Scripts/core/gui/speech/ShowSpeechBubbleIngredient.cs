using UnityEngine;
using System.Collections;
using Messages;

public class ShowSpeechBubbleIngredient : Ingredient
{
	public SpeechMenu speechMenu;
	public string localizationId;
	public float durationTime;
	public bool skippable = true;

	private float elapsedTime = 0.0f;

	public override void Prepare()
	{
		base.Prepare();

		EventMessageHandler backgroundButtonTappedEventHandler = new EventMessageHandler(this, OnBackgroundButtonTapped);
		EventMessageManager.instance.AddHandler(typeof(BackgroundButtonTappedEvent).Name, backgroundButtonTappedEventHandler);

		elapsedTime = 0.0f;
		speechMenu.ShowSpeechBubble(localizationId);
	}


	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			elapsedTime += deltaTime;
			if (elapsedTime >= durationTime)
			{
				Complete();
			}
		}

		if (status == CookbookStatus.Success)
		{
			EventMessageManager.instance.RemoveHandler(typeof(BackgroundButtonTappedEvent).Name, this);
		}

		return status;
	}

	private void OnBackgroundButtonTapped(EventMessage eventMessage)
	{
		if (skippable)
		{
			Complete();
		}
	}

	private void Complete()
	{
		speechMenu.HideSpeechBubble();
		status = CookbookStatus.Success;
	}
}

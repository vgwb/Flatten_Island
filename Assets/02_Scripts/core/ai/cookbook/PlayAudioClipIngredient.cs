using UnityEngine;

public class PlayAudioClipIngredient : Ingredient
{
	public AudioClip audioClip;

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			if (audioClip != null)
			{
				AudioManager.instance.PlayOneShot(audioClip, EAudioChannelType.Sfx);
			}

			status = CookbookStatus.Success;
		}

		return status;
	}
}

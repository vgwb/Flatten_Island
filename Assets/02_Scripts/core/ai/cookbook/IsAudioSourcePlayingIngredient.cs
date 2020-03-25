using UnityEngine;

public class IsAudioSourcePlayingIngredient : Ingredient
{
	public AudioSource audioSource; 

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			if (audioSource.isPlaying)
			{
				status = CookbookStatus.Success;
			}
			else
			{
				status = CookbookStatus.Failure;
			}
		}

		return status;
	}
}

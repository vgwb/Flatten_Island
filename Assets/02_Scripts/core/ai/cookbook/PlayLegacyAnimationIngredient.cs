using UnityEngine;

public class PlayLegacyAnimationIngredient : Ingredient
{
	public Animation legacyAnimation;
	public AnimationClip animationClip;

	private bool started;

	public override void Prepare()
	{
		base.Prepare();
		started = false;
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			if (!started)
			{
				started = true;
				if (animationClip == null)
				{
					legacyAnimation.Play();
				}
				else
				{
					legacyAnimation.Play(animationClip.name);
				}
			}
			else
			{
				if (!legacyAnimation.isPlaying)
				{
					status = CookbookStatus.Success;
				}
			}
		}

		return status;
	}
}

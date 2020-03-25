using UnityEngine;

public class PlayAnimationIngredient : Ingredient
{
	public Animator animator;
	public AnimationClip animationClip;

	private int animatorLayer = 0;

	public override void Prepare()
	{
		base.Prepare();
		AnimatorUtils.SetTrigger(animator, animationClip.name);
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			if (AnimatorUtils.IsAnimationStarted(animator, animationClip.name, animatorLayer))
			{
				if (!AnimatorUtils.IsPlayingAnimation(animator, animationClip.name, animatorLayer))
				{
					status = CookbookStatus.Success;
				}
			}
		}

		return status;
	}
}

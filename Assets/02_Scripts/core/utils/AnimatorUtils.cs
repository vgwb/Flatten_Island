using UnityEngine;

public static class AnimatorUtils
{
	public const string BASE_LAYER_NAME = "Base Layer";

	public static void SetTrigger(Animator animator, string animationName, int animatorLayer = 0, string layerName = BASE_LAYER_NAME)
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(animatorLayer);
		if (stateInfo.IsName(layerName + "." + animationName))
		{
			if (stateInfo.loop)
			{
				return;
			}

			if (stateInfo.normalizedTime < 1.0f)
			{
				return;
			}
		}

		ResetAllTriggers(animator);
		animator.SetTrigger(animationName);
	}

	private static void ResetAllTriggers(Animator animator)
	{
		foreach (AnimatorControllerParameter p in animator.parameters)
		{
			if (p.type == AnimatorControllerParameterType.Trigger)
			{
				animator.ResetTrigger(p.name);
			}
		}
	}

	public static bool IsPlayingAnimation(Animator animator, string animationName, int animatorLayer = 0, string layerName = BASE_LAYER_NAME)
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(animatorLayer);
		if (stateInfo.IsName(layerName + "." + animationName))
		{
			if (animator.IsInTransition(animatorLayer))
			{
				//We assume a state won't exit and re-enter in itself. 
				//So if state matches then it's transitioning outside the state
				return false;
			}

			if (!stateInfo.loop)
			{
				return stateInfo.normalizedTime < 1f;
			}

			return true;
		}

		return false;
	}

	public static bool IsAnimationStarted(Animator animator, string animationName, int animatorLayer, string layerName = BASE_LAYER_NAME)
	{
		AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(animatorLayer);
		return stateInfo.normalizedTime > 0f && stateInfo.IsName(layerName + "." + animationName);
	}
}
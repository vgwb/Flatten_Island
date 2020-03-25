using UnityEngine;

public class SetAnimatorFloatParameterIngredient : Ingredient
{
	public Animator animator;
	public string parameterName;
	public float parameterValue;

	public override void Prepare()
	{
		base.Prepare();
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			animator.SetFloat(parameterName, parameterValue);
			status = CookbookStatus.Success;
		}

		return status;
	}
}

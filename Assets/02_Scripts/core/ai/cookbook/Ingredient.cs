using UnityEngine;

public class Ingredient : MonoBehaviour
{
	public Recipe recipe { get; private set; }

	protected CookbookStatus status;

	public virtual void Prepare()
	{
		status = CookbookStatus.Running;
	}

	public virtual CookbookStatus Use(float deltaTime)
	{
		return CookbookStatus.Success;
	}

	public void SetRecipe(Recipe recipe)
	{
		this.recipe = recipe;
	}
}

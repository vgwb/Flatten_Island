public class AnimatorEventPlayRecipe : BaseBehaviour
{
	public AnimatorEventChef animatorEventChef;
	public AnimatorEventChef foreignAnimatorEventChef;

	public void PlayRecipe(string recipeName)
	{
		animatorEventChef.CookRecipeByName(recipeName);
	}

	public void PlayForeignRecipe(string recipeName)
	{
		foreignAnimatorEventChef.CookRecipeByName(recipeName);
	}
}
public class CheatManager : MonoSingleton
{
	public bool showSuggestionParameters;
	public bool forceIntro;
	public bool skipLogo;

	public static CheatManager instance
	{
		get
		{
			return GetInstance<CheatManager>();
		}
	}

	public void ShowCheatMenu()
	{
#if CHEAT_DEBUG
		CheatMenu.Show();
#endif
	}

	public void ToggleSuggestionParameters()
	{
#if CHEAT_DEBUG
		showSuggestionParameters = !showSuggestionParameters;
#endif
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CheatManager : MonoSingleton
{
	public bool showSuggestionParameters;
	public bool forceIntro;

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

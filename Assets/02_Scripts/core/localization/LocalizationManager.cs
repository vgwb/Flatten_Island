public class LocalizationManager : Singleton
{
	public static LocalizationManager instance
	{
		get
		{
			return GetInstance<LocalizationManager>();
		}
	}

	public string GetLocalizedText(string localizationKey)
	{
		if (localizationKey.Equals("Dungeon Level 1"))
		{
			return "DUNGEON CORRIDORS LEVEL 01";
		}

		if (localizationKey.Equals("ExitFromTheDungeon"))
		{
			return "EXIT FROM THE DUNGEON!";
		}

		return localizationKey;
	}
}

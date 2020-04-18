public class PlayerSettings : ISavable
{
	public bool skipIntro;
	public bool showTutorial;
	public bool musicOn;
	public bool sfxOn;

	private string languageId;

	public PlayerSettings()
	{
		skipIntro = false;
		showTutorial = true;
		musicOn = true;
		sfxOn = true;
		languageId = null;
	}

	public bool HasLanguageId()
	{
		return !string.IsNullOrEmpty(languageId);
	}

	public void SetLanguageId(string languageId)
	{
		this.languageId = languageId;
	}

	public string GetLanguageId()
	{
		return languageId;
	}

	public GameData WriteSaveData()
	{
		PlayerSettingsData playerSettingsData = new PlayerSettingsData();
		playerSettingsData.skipIntro = skipIntro;
		playerSettingsData.showTutorial = showTutorial;
		playerSettingsData.musicOn = musicOn;
		playerSettingsData.sfxOn = sfxOn;
		playerSettingsData.languageId = languageId;

		return playerSettingsData;
	}

	public void ReadSaveData(GameData gameData)
	{
		PlayerSettingsData playerSettingsData = gameData as PlayerSettingsData;
		skipIntro = playerSettingsData.skipIntro;
		showTutorial = playerSettingsData.showTutorial;
		musicOn = playerSettingsData.musicOn;
		sfxOn = playerSettingsData.sfxOn;
		languageId = playerSettingsData.languageId;
	}
}
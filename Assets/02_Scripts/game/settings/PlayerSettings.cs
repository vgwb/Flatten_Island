public class PlayerSettings : ISavable
{
	public bool skipIntro;
	public bool showTutorial;
	public bool musicOn;
	public bool sfxOn;

	public PlayerSettings()
	{
		skipIntro = false;
		showTutorial = true;
		musicOn = true;
		sfxOn = true;
	}

	public GameData WriteSaveData()
	{
		PlayerSettingsData playerSettingsData = new PlayerSettingsData();
		playerSettingsData.skipIntro = skipIntro;
		playerSettingsData.showTutorial = showTutorial;
		playerSettingsData.musicOn = musicOn;
		playerSettingsData.sfxOn = sfxOn;

		return playerSettingsData;
	}

	public void ReadSaveData(GameData gameData)
	{
		PlayerSettingsData playerSettingsData = gameData as PlayerSettingsData;
		skipIntro = playerSettingsData.skipIntro;
		showTutorial = playerSettingsData.showTutorial;
		musicOn = playerSettingsData.musicOn;
		sfxOn = playerSettingsData.sfxOn;
	}
}
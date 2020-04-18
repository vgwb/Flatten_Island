using System;

[Serializable]
public class PlayerSettingsData : GameData
{
	public bool skipIntro;
	public bool showTutorial;
	public bool musicOn;
	public bool sfxOn;
	public string languageId;
}
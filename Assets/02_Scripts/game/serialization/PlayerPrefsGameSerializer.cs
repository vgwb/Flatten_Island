using UnityEngine;
using System.Collections;

public class PlayerPrefsGameSerializer : GameSerializer
{
	public void DeleteSaveGame(ISaveGameStorage saveGameStorage)
	{
		PlayerPrefs.DeleteAll();
	}

	public void WriteSaveGame(ISaveGameStorage saveGameStorage, LocalPlayer localPlayer)
	{
		PlayerPrefs.SetInt("skipIntro", localPlayer.skipIntro ? 1 : 0);

	}

	public void ReadSaveGame(ISaveGameStorage saveGameStorage, out LocalPlayer localPlayer)
	{
		localPlayer = new LocalPlayer();

		if (PlayerPrefs.HasKey("skipIntro"))
		{
			int skipIntro = PlayerPrefs.GetInt("skipIntro");
			if (skipIntro == 1)
			{
				localPlayer.skipIntro = true;
			}
		}
	}
}

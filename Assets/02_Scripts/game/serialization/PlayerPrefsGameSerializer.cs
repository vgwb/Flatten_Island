using UnityEngine;
using System.Collections;

public class PlayerPrefsGameSerializer : GameSerializer
{
	public void DeleteSaveGame()
	{
		PlayerPrefs.DeleteAll();
	}

	public void WriteSaveGame(LocalPlayer localPlayer)
	{
		PlayerPrefs.SetInt("skipIntro", localPlayer.skipIntro ? 1 : 0);

	}

	public void ReadSaveGame(out LocalPlayer localPlayer)
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

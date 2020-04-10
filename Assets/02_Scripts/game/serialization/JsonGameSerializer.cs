using System;
using UnityEngine;

public class JsonGameSerializer : GameSerializer
{
	private const string SAVE_GAME_FILE = "json_savegame";
	private const string SAVE_GAME_BACKUP_FILE = "json_backup_savegame";

	public void DeleteSaveGame(ISaveGameStorage saveGameStorage)
	{
		saveGameStorage.Delete(SAVE_GAME_FILE);
		saveGameStorage.Delete(SAVE_GAME_BACKUP_FILE);
	}

	public void WriteSaveGame(ISaveGameStorage saveGameStorage, LocalPlayer localPlayer)
	{
		string jsonSaveGame = GetSaveGame(localPlayer).ToString();
		saveGameStorage.Write(SAVE_GAME_FILE, jsonSaveGame);
	}

	public void ReadSaveGame(ISaveGameStorage saveGameStorage, out LocalPlayer localPlayer)
	{
		localPlayer = new LocalPlayer();

		string jsonSaveGame = ReadJsonFromDataStorage(saveGameStorage);
		if (jsonSaveGame != null)
		{
			LocalPlayerData localPlayerData = JsonUtility.FromJson<LocalPlayerData>(jsonSaveGame);
			if (localPlayerData != null)
			{
				saveGameStorage.Write(SAVE_GAME_BACKUP_FILE, jsonSaveGame);
				localPlayer.ReadSaveData(localPlayerData);
			}
		}
	}

	private string GetSaveGame(LocalPlayer localPlayer)
	{
		LocalPlayerData localPlayerData = localPlayer.WriteSaveData();
		return JsonUtility.ToJson(localPlayerData);
	}

	private string ReadJsonFromDataStorage(ISaveGameStorage saveGameStorage)
	{
		return saveGameStorage.Read(SAVE_GAME_FILE);
	}

}
using System;
using UnityEngine;

public class JsonGameSerializer : ISaveGameSerializer
{
	private const string SAVE_GAME_FILE = "json_savegame";
	private const string SAVE_GAME_BACKUP_FILE = "json_backup_savegame";

	public void DeleteSaveGame(ISaveGameStorage saveGameStorage)
	{
		saveGameStorage.Delete(SAVE_GAME_FILE);
		saveGameStorage.Delete(SAVE_GAME_BACKUP_FILE);
	}

	public void WriteSaveGame(ISaveGameStorage saveGameStorage, ISavable savableObject)
	{
		LocalPlayer localPlayer = savableObject as LocalPlayer;

		string jsonSaveGame = GetSaveGame(localPlayer).ToString();
		saveGameStorage.Write(SAVE_GAME_FILE, jsonSaveGame);
	}

	public ISavable ReadSaveGame(ISaveGameStorage saveGameStorage)
	{
		string jsonSaveGame = ReadJsonFromDataStorage(saveGameStorage);
		if (jsonSaveGame != null)
		{
			LocalPlayerData localPlayerData = JsonUtility.FromJson<LocalPlayerData>(jsonSaveGame);
			if (localPlayerData != null)
			{
				LocalPlayer localPlayer = new LocalPlayer();
				saveGameStorage.Write(SAVE_GAME_BACKUP_FILE, jsonSaveGame);
				localPlayer.ReadSaveData(localPlayerData);
				return localPlayer;
			}
		}

		return null;
	}

	private string GetSaveGame(LocalPlayer localPlayer)
	{
		LocalPlayerData localPlayerData = localPlayer.WriteSaveData() as LocalPlayerData;
		return JsonUtility.ToJson(localPlayerData);
	}

	private string ReadJsonFromDataStorage(ISaveGameStorage saveGameStorage)
	{
		return saveGameStorage.Read(SAVE_GAME_FILE);
	}

}
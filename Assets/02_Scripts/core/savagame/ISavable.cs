public interface ISavable
{
	GameData WriteSaveData();
	void ReadSaveData(GameData gameData);
}
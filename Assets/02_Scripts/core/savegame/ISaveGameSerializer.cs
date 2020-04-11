public interface ISaveGameSerializer
{
	void DeleteSaveGame(ISaveGameStorage saveGameStorage);
	void WriteSaveGame(ISaveGameStorage saveGameStorage, ISavable savableObject);
	ISavable ReadSaveGame(ISaveGameStorage saveGameStorage);
}

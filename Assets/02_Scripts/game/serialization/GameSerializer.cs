public interface GameSerializer
{
	void DeleteSaveGame(ISaveGameStorage saveGameStorage);
	void WriteSaveGame(ISaveGameStorage saveGameStorage, LocalPlayer localPlayer);
	void ReadSaveGame(ISaveGameStorage saveGameStorage, out LocalPlayer localPlayer);
}

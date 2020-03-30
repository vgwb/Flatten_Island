public interface GameSerializer
{
	void DeleteSaveGame();
	void WriteSaveGame(LocalPlayer localPlayer);
	void ReadSaveGame(out LocalPlayer localPlayer);
}

public abstract class Player : ISavable
{
	public abstract GameData WriteSaveData();
	public abstract void ReadSaveData(GameData gameData);
}
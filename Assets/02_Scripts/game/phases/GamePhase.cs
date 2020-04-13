using UnityEngine;

public class GamePhase : ISavable
{
	private GamePhaseXmlModel gamePhaseXmlModel;

	public void StartPhase(int gamePhaseId)
	{
		gamePhaseXmlModel = XmlModelManager.instance.FindModel<GamePhaseXmlModel>(gamePhaseId);
	}

	public GameData WriteSaveData()
	{
		GamePhaseData gamePhaseData = new GamePhaseData();
		gamePhaseData.gamePhaseId = gamePhaseXmlModel.id;
		return gamePhaseData;
	}

	public void ReadSaveData(GameData gameData)
	{
		GamePhaseData gamePhaseData = gameData as GamePhaseData;
		gamePhaseXmlModel = XmlModelManager.instance.FindModel<GamePhaseXmlModel>(gamePhaseData.gamePhaseId);
	}
}
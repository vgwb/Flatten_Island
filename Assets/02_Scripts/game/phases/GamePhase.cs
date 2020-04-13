using UnityEngine;
using Messages;

public class GamePhase : ISavable
{
	private GamePhaseXmlModel gamePhaseXmlModel;

	private int startDay;

	public void Start(int gamePhaseId, int startDay)
	{
		gamePhaseXmlModel = XmlModelManager.instance.FindModel<GamePhaseXmlModel>(gamePhaseId);
		this.startDay = startDay;
	}

	public void Resume()
	{
	}

	public void Stop()
	{

	}

	public string GetName()
	{
		return LocalizationManager.instance.GetText(gamePhaseXmlModel.name);
	}

	public int GetNextPhaseId()
	{
		return gamePhaseXmlModel.nextPhaseId;
	}

	public int GetStartDay()
	{
		return startDay;
	}

	public bool IsFinished(GameSession gameSession)
	{
		if (gamePhaseXmlModel.endConditions == null)
		{
			return false;
		}

		return gamePhaseXmlModel.endConditions.IsSatisfied(gameSession);
	}

	public GameData WriteSaveData()
	{
		GamePhaseData gamePhaseData = new GamePhaseData();
		gamePhaseData.gamePhaseId = gamePhaseXmlModel.id;
		gamePhaseData.startDay = startDay;
		return gamePhaseData;
	}

	public void ReadSaveData(GameData gameData)
	{
		GamePhaseData gamePhaseData = gameData as GamePhaseData;
		gamePhaseXmlModel = XmlModelManager.instance.FindModel<GamePhaseXmlModel>(gamePhaseData.gamePhaseId);
		startDay = gamePhaseData.startDay;
	}
}
public class GamePhaseStartEnterCompletedEvent : EventObject
{
	public GamePhaseXmlModel gamePhaseXmlModel;

	public static GamePhaseStartEnterCompletedEvent CreateInstance(GamePhaseXmlModel gamePhaseXmlModel)
	{
		GamePhaseStartEnterCompletedEvent eventObject = CreateInstance<GamePhaseStartEnterCompletedEvent>();
		eventObject.name = typeof(GamePhaseStartEnterCompletedEvent).Name;
		eventObject.gamePhaseXmlModel = gamePhaseXmlModel;
		return eventObject;
	}
}

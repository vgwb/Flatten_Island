public class GamePhaseStartExitCompletedEvent : EventObject
{
	public GamePhaseXmlModel gamePhaseXmlModel;

	public static GamePhaseStartExitCompletedEvent CreateInstance(GamePhaseXmlModel gamePhaseXmlModel)
	{
		GamePhaseStartExitCompletedEvent eventObject = CreateInstance<GamePhaseStartExitCompletedEvent>();
		eventObject.name = typeof(GamePhaseStartEnterCompletedEvent).Name;
		eventObject.gamePhaseXmlModel = gamePhaseXmlModel;
		return eventObject;
	}
}

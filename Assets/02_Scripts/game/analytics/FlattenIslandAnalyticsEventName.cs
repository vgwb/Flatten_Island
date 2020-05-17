using UnityEngine;

public class FlattenIslandAnalyticsEventName : AnalyticsEventName
{
	public const string StartNewGame = "StartNewGame";
	public const string QuitGame = "QuitGame";
	public const string WinGame = "WinGame";
	public const string LoseGameMoney = "LoseGameMoney";
	public const string LoseGamePubOp = "LoseGamePubOp";
	public const string LoseGamePatients = "LoseGamePatients";

	public FlattenIslandAnalyticsEventName(string eventName)
	{
		SetEvent(eventName);
	}

	private void SetEvent(string eventName)
	{
		bool isEventNameDefined = false;

		if (eventName.Equals(StartNewGame))
		{
			isEventNameDefined = true;
		}

		if (eventName.Equals(QuitGame))
		{
			isEventNameDefined = true;
		}

		if (eventName.Equals(WinGame))
		{
			isEventNameDefined = true;
		}

		if (eventName.Equals(LoseGameMoney))
		{
			isEventNameDefined = true;
		}

		if (eventName.Equals(LoseGamePubOp))
		{
			isEventNameDefined = true;
		}

		if (eventName.Equals(LoseGamePatients))
		{
			isEventNameDefined = true;
		}

		if (isEventNameDefined)
		{
			EventName = eventName;
		}
		else
		{
			EventName = Undefined;
			Debug.LogError("FlattenIslandAnalyticsEventName - event name:" + eventName + " is not defined!");
		}
	}
}
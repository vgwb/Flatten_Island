using UnityEngine;

public class FlattenIslandAnalyticsEventParameter : AnalyticsEventParameter
{
	public const string PlayerId = "PlayerId";
	public const string TotGamesPlayed = "TotGamesPlayed";
	public const string GameCurrentDay = "GameCurrentDay";

	public FlattenIslandAnalyticsEventParameter(string eventParameter)
	{
		SetParameter(eventParameter);
	}

	private void SetParameter(string eventParameter)
	{
		bool isEventParameterDefined = false;

		if (eventParameter.Equals(TotGamesPlayed))
		{
			isEventParameterDefined = true;
		}

		if (eventParameter.Equals(GameCurrentDay))
		{
			isEventParameterDefined = true;
		}

		if (eventParameter.Equals(PlayerId))
		{
			isEventParameterDefined = true;
		}

		if (isEventParameterDefined)
		{
			EventParameter = eventParameter;
		}
		else
		{
			EventParameter = Undefined;
			Debug.LogError("FlattenIslandAnalyticsEventParameter - event parameter:" + eventParameter + " is not defined!");
		}
	}
}
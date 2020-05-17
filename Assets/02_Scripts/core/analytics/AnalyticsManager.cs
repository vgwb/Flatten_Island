using System.Collections.Generic;

public class AnalyticsManager
{
	public static void LogEvent(string eventName, Dictionary<string, object> parameters)
	{
		UnityEngine.Analytics.AnalyticsEvent.Custom(eventName, parameters);
	}

	public static void FlushEvents()
	{
		UnityEngine.Analytics.Analytics.FlushEvents();
	}
}
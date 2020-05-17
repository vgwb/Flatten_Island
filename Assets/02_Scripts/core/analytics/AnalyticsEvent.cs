using System.Collections.Generic;

public abstract class AnalyticsEvent
{
	public AnalyticsEventName eventName;
	public Dictionary<AnalyticsEventParameter, object> eventParams;

	public AnalyticsEvent(AnalyticsEventName eventName)
	{
		this.eventName = eventName;
		eventParams = new Dictionary<AnalyticsEventParameter, object>();
	}

	public void AddParam(AnalyticsEventParameter key, object content)
	{
		eventParams[key] = content;
	}

	private Dictionary<string, object> ConvertParams()
	{
		Dictionary<string, object> convertedParams = new Dictionary<string, object>();
		foreach (var pair in eventParams)
		{
			convertedParams.Add(pair.Key.EventParameter, pair.Value);
		}
		return convertedParams;
	}

	public void Send()
	{
		AnalyticsManager.LogEvent(eventName.EventName, ConvertParams());
		AnalyticsManager.FlushEvents();
	}
}
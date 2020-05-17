using UnityEngine;

public abstract class AnalyticsEventName
{
	public const string Undefined = "Undefined";
	public string EventName { get; protected set; }
}
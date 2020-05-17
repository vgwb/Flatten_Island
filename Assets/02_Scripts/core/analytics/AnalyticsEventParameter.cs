using UnityEngine;

public abstract class AnalyticsEventParameter
{
	public const string Undefined = "Undefined";

	public string EventParameter { get; protected set; }
}
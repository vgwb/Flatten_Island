using UnityEngine;
using System.Collections;

public class Suggestion
{
	public Advisor advisor { get; set; }
	public string title { get; set; }
	public string description { get; set; }
	public int growthRateModifier { get; set; }
	public int capacityModifier { get; set; }
	public int patientsModifier { get; set; }
	public int moneyModifier { get; set; }
	public float publicOpinionModifier { get; set; }

	public Suggestion()
	{
	}
}

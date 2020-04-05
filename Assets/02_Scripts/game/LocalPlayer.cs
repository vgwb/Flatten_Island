using UnityEngine;
using System.Collections;

public class LocalPlayer
{
	public const int MAX_DAYS = 110;
	public const int MAX_PATIENTS = 10000;

	public bool skipIntro;

	public int day { get; set; }
	public int[] patients { get; set; }
	public int vaccineDevelopment { get; set; }
	public int growthRate { get; set; }
	public int capacity { get; set; }
	public int money { get; set; }
	public float publicOpinion { get; set; }
	public Suggestion[] suggestions { get; private set; }
	public Suggestion adviced { get; private set; }

	public LocalPlayer()
	{
		patients = new int[MAX_DAYS];
	}

	public void Init()
	{
		// TODO provisional initial state
		day = 1;
		vaccineDevelopment=0;
		patients[0] = 1000;
		growthRate = 5;
		capacity = 1000;
		money = 10000;
		publicOpinion = 0.5f;

		CreateAdvisorSuggestions();
	}

	public void IncreaseDayAcceptSuggestion()
	{
		Debug.Log("IncreaseDayAcceptSuggestion");
		Debug.Log("Adviced" + adviced.ToString());
		money += adviced.moneyModifier;
		growthRate += adviced.growthRateModifier;
		publicOpinion += adviced.publicOpinionModifier;
		capacity += adviced.capacityModifier;
		int patientsIncrease = (patients[day - 1] * growthRate) / 100;
		patients[day] = patients[day - 1] + patientsIncrease + adviced.patientsModifier;
		IncreaseDay();
	}

	public void IncreaseDayRejectSuggestion()
	{
		int patientsIncrease = (patients[day - 1] * growthRate) / 100;
		patients[day] = patients[day - 1] + patientsIncrease;
		IncreaseDay();
	}

	public void IncreaseDay()
	{
		money += patients[day] * 3 - capacity * 2;
		vaccineDevelopment++;
		day++;
		CreateAdvisorSuggestions();
	}

	private void CreateAdvisorSuggestions()
	{
		SuggestionFactory sf = GameManager.instance.suggestionFactory;
		suggestions = sf.CreateNextSuggestions(adviced, this);
		adviced = suggestions[0]; // TODO allow the player to choose. Bypassing
	}
}

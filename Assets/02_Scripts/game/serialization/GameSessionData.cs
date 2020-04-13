using System;

[Serializable]
public class GameSessionData : GameData
{
	public int day;
	public int vaccineDevelopment;
	public int growthRate;
	public int capacity;
	public int money;
	public int publicOpinion;
	public int[] patients;
	public int[] advisorIds;

	public GamePhaseData gamePhaseData;
}
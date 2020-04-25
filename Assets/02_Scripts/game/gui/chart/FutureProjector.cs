using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FutureProjector : MonoSingleton
{
	// Public options
	public int phase1FinalDay;
	public int phase2FinalDay;
	public int phase1GrowthDelta;
	public int phase2GrowthDelta;
	public int phase3GrowthDelta;
	public int phase1CapacityDelta;
	public int phase2CapacityDelta;
	public int phase3CapacityDelta;

	public static FutureProjector instance
	{
		get
		{
			return GetInstance<FutureProjector>();
		}
	}

	public int GetPredictedPatients(int day, GameSession session)
	{
		if (session == null) return 0;

		int currentDay = session.day;
		int capacity = session.capacity;
		int growth = session.growthRate;
		int patients = session.patients[currentDay - 1];
					
		int predictedPatients = ProjectWithMethod1(day, currentDay, capacity, growth, patients);
		
		return predictedPatients;
	}

	public int ProjectWithMethod1(int day, int currentDay, int capacity, int growth, int patients)
	{
		int targetPatients = patients + (patients * growth) / 100; // calculate today with current
		int capacityOnDate = capacity;
		int growthOnDate = growth;

		// Calculate the predicted patients base on growth for future days
		for (int d = currentDay + 1; d <= day; d++)
		{
			growthOnDate += GetPredictedGrowthDeltaForDay(d);
			capacityOnDate += GetPredictedCapacityDeltaForDay(d);
			targetPatients += (targetPatients * growthOnDate) / 100;
		}

		// Normalise those patients to the current capacity based on the predicted capacity
		float capacityRatio = capacity / (float)capacityOnDate;
		int normalisedPatients = (int)(targetPatients * capacityRatio);
		return normalisedPatients;
	}

	public int GetPredictedGrowthDeltaForDay(int day)
	{
		if (day < phase1FinalDay) return phase1GrowthDelta;
		if (day < phase2FinalDay) return phase2GrowthDelta;
		return phase3GrowthDelta;
	}

	public int GetPredictedCapacityDeltaForDay(int day)
	{
		if (day < phase1FinalDay) return phase1CapacityDelta;
		if (day < phase2FinalDay) return phase2CapacityDelta;
		return phase3CapacityDelta;
	}

}

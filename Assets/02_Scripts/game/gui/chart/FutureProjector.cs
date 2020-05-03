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
	public int capacityInterpolationDays;

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
		
		// Debug.Log("GetPredictedPatients: " + day + ": " + currentDay + " currentDay, " + patients + " current patients " + predictedPatients + " predictedPatients");
		return predictedPatients;
	}

	public int ProjectWithMethod1(int day, int currentDay, int capacity, int growth, int patients)
	{
		int targetPatients = patients + (patients * growth) / 100; // calculate today with current growth
		int capacityOnDate = capacity;
		int growthOnDate = growth;

		// Calculate the predicted patients base on growth for future days
		for (int d = currentDay + 1; d <= day; d++)
		{
			growthOnDate += GetPredictedGrowthDeltaForDay(d);
			capacityOnDate += GetPredictedCapacityDeltaForDayInterpolating(d, currentDay, capacity);
			targetPatients += (targetPatients * growthOnDate) / 100;
			// Debug.Log("Prediction: " + d + ": " + growthOnDate + "%, " + capacityOnDate + " beds, " + targetPatients + " patients");
		}

		// Normalise those patients to the current capacity based on the predicted capacity
		float capacityRatio = capacity / (float)capacityOnDate;
		int normalisedPatients = (int)(targetPatients * capacityRatio);
		return normalisedPatients;
	}

	public bool IsCurvePredictedToOverflow(GameSession session)
	{
		if (session == null) return false;

		int currentDay = session.day;
		int capacity = session.capacity;
		int growth = session.growthRate;
		int patients = session.patients[currentDay - 1];
					
		// predicted moving variables
		int targetPatients = patients + (patients * growth) / 100; // calculate today with current growth
		int capacityOnDate = capacity;
		int growthOnDate = growth;

		for (int d = currentDay + 1; d <= GameSession.MAX_DAYS; d++)
		{
			growthOnDate += GetPredictedGrowthDeltaForDay(d);
			capacityOnDate += GetPredictedCapacityDeltaForDayInterpolating(d, currentDay, capacity);
			targetPatients += (targetPatients * growthOnDate) / 100;
			if (targetPatients > capacityOnDate) return true;
		}

		return false;
	}

	public int GetPredictedGrowthDeltaForDay(int day)
	{
		if (day < phase1FinalDay) return phase1GrowthDelta;
		if (day < phase2FinalDay) return phase2GrowthDelta;
		return phase3GrowthDelta;
	}

	public int GetPredictedCapacityDeltaForDayInterpolating(int day, int currentDay, int currentCapacity)
	{
		float averageDeltaInPast = currentCapacity / (float)currentDay;
		float expectedDeltaInFuture = phase3CapacityDelta;
		if (day < phase1FinalDay) expectedDeltaInFuture = phase1CapacityDelta;
		if (day < phase2FinalDay) expectedDeltaInFuture = phase2CapacityDelta;

		// interpolate past growth with expected growth during the following capacityInterpolationDays days
		int remainingDays = GetMaxDays(currentDay) - currentDay;
		int interpolatingDays = Math.Min(remainingDays, capacityInterpolationDays);

		if (day < currentDay + interpolatingDays)
		{
			float futureWeight = (day-currentDay) / (float)interpolatingDays;
			float pastWeight = 1.0f - futureWeight;
			float weightedCapacity = pastWeight * averageDeltaInPast + futureWeight * expectedDeltaInFuture;
			// Debug.Log("Interpolating weightedCapacity: " + weightedCapacity + " day " + day + " futureWeight " + futureWeight);
			return (int)weightedCapacity;
		}
		return (int)expectedDeltaInFuture;
	}

	public int GetPredictedCapacityDeltaForDay(int day)
	{
		if (day < phase1FinalDay) return phase1CapacityDelta;
		if (day < phase2FinalDay) return phase2CapacityDelta;
		return phase3CapacityDelta;
	}

	private int GetMaxDays(int currentDay)
	{
		return Math.Max(currentDay, GameSession.MAX_DAYS);
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;
using ProtoTurtle.BitmapDrawing;

public class PatientsPanelSelector : MonoSingleton
{
	private static Vector3 invisiblePosition = new Vector3(10000f,10000f,0f);

	public GameObject patientsOverflow;
	public GameObject patientsStraight;
	public GameObject patientsBelowRight;
	public GameObject patientsAbove;
	public GameObject patientsAboveRight;

	public int initialPeriod;
	public int peakGrowthThreshold;

	public static PatientsPanelSelector instance
	{
		get
		{
			return GetInstance<PatientsPanelSelector>();
		}
	}

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();
	}

	public GameObject SelectPanel()
	{
		if (GameManager.instance.localPlayer.gameSession == null) return patientsStraight;

		int currentDay = GameManager.instance.localPlayer.gameSession.day;
		int growth = GameManager.instance.localPlayer.gameSession.growthRate;
		int[] patients = GameManager.instance.localPlayer.gameSession.patients;
		int capacity = GameManager.instance.localPlayer.gameSession.capacity;

		int currentPatients;
		if (currentDay == 0)
		{
			currentPatients = GameManager.instance.localPlayer.gameSession.GetPreviousDayPatientsForTutorialDayZero();
		}
		else
		{
			currentPatients = GameManager.instance.localPlayer.gameSession.patients[currentDay - 1];
		}

		int peakDay = FindPeakDay(patients, currentDay);

		if (IsOverflow(currentPatients, capacity))
		{
			//Debug.Log("returning IsOverflow -> patientsOverflow");
			return patientsOverflow;
		}
		if (IsInitialPeriod(currentDay))
		{
			//Debug.Log("returning IsInitialPeriod -> patientsStraight");
			return patientsStraight;
		}
		if (IsOverMaxExpectedDays(currentDay))
		{
			//Debug.Log("returning IsOverMaxExpectedDays -> patientsAbove");
			return patientsAbove;
		}
		if (IsCloseToAPeak(growth))
		{
			//Debug.Log("returning IsCloseToAPeak -> patientsAbove");
			return patientsAboveRight;
		}
		if (!HasPassedPeak(currentDay, peakDay))
		{
			//Debug.Log("returning not HasPassedPeak -> patientsBelowRight");
			return patientsBelowRight;
		}
		if (IsGrowingAfterPeak(currentDay, peakDay, growth))
		{
			//Debug.Log("returning IsGrowingAfterPeak -> patientsAbove");
			return patientsAbove;
		}
		
		// going down after peak
		//Debug.Log("returning going down after peak -> patientsAboveLeft");
		return patientsAboveRight;
	}

	private bool IsOverflow(int patients, int capacity)
	{
		return patients > capacity;
	}

	private bool IsInitialPeriod(int currentDay)
	{
		return currentDay < initialPeriod;
	}

	private bool IsOverMaxExpectedDays(int currentDay)
	{
		int intoOvertime = GameManager.instance.localPlayer.gameSession.maxDays - GameSession.EXTRA_MAX_DAYS;
		return currentDay > intoOvertime;
	}

	private bool IsCloseToAPeak(int growth)
	{
		return Math.Abs(growth) < peakGrowthThreshold;
	}

	private bool IsAtThePeak(int currentDay, int peakDay, int growth)
	{
		return peakDay == currentDay-1 && growth < 0;
	}

	private bool HasPassedPeak(int currentDay, int peakDay)
	{
		return peakDay < currentDay-1;
	}

	private bool IsGrowingAfterPeak(int currentDay, int peakDay, int growth)
	{
		return HasPassedPeak(currentDay, peakDay) && growth > 0;
	}

	private int FindPeakDay(int[] patients, int currentDay)
	{
		int peakDay = 0;
		int max = patients[0];

		for (int d=1; d<currentDay; d++)
		{
			if (patients[d] > max)
			{
				max = patients[d];
				peakDay = d;
			}
		}
		return peakDay;
	}

	public void HidePanels()
	{	
		HidePanel(patientsOverflow);
		HidePanel(patientsStraight);
		HidePanel(patientsBelowRight);
		HidePanel(patientsAbove);
		HidePanel(patientsAboveRight);
	}

	public void HidePanel(GameObject panel)
	{	
		panel.transform.localPosition = invisiblePosition;
	}
}

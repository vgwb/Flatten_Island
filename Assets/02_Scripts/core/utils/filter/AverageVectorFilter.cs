using UnityEngine;
using System;
using System.Collections.Generic;

public class AverageVectorFilter
{
	private List<List<float>> rollingAverage;
	private int sampleSize;

	public AverageVectorFilter(int sampleSize)
	{
		rollingAverage = new List<List<float>>();
		rollingAverage.Add(new List<float>());
		rollingAverage.Add(new List<float>());
		rollingAverage.Add(new List<float>());
		this.sampleSize = sampleSize;
	}

	public List<float> Roll(List<float> list, float newMember)
	{
		if (list.Count == sampleSize)
		{
			list.RemoveAt(0);
		}
		list.Add(newMember);
		return list;
	}

	public float AverageList(List<float> tallyUp)
	{

		float total = 0;
		foreach (float item in tallyUp)
		{
			total += item;
		}
		total = total / tallyUp.Count;

		return total;
	}

	public Vector3 Read(Vector3 point)
	{
		rollingAverage[0] = Roll(rollingAverage[0], point.x);
		rollingAverage[1] = Roll(rollingAverage[1], point.y);
		rollingAverage[2] = Roll(rollingAverage[2], point.z);

		Vector3 smoothResult = Vector3.zero;

		smoothResult.x = AverageList(rollingAverage[0]);
		smoothResult.y = AverageList(rollingAverage[1]);
		smoothResult.z = AverageList(rollingAverage[2]);

		return smoothResult;
	}
}

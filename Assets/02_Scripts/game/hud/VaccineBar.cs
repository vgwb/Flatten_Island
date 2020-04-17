﻿using UnityEngine;

public class VaccineBar : MonoBehaviour
{
	private const float BORDER_MOVEMENT_FACTOR = 100f;

	public Transform barTransform;
	public Transform rightBorderTransform;

	private Vector3 rightBorderStartPosition;

	private void Awake()
	{
		rightBorderStartPosition = rightBorderTransform.position;
	}

	public void Initialize()
	{
		barTransform.localScale = new Vector3(0.0f, 1.0f);
	}

	public void UpdateBar(float percentage)
	{
		barTransform.localScale = new Vector3(percentage, 1.0f);
		rightBorderTransform.position = new Vector3(rightBorderStartPosition.x + (BORDER_MOVEMENT_FACTOR * percentage), rightBorderTransform.position.y, rightBorderTransform.position.z);
	}
}
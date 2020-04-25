using System;
using UnityEngine;
using Messages;

public class ToggleButton : MonoBehaviour
{
	public GameObject toggleOn;
	public GameObject toggleOff;

	private bool state;

	protected void SetState(bool value)
	{
		state = value;
		toggleOn.SetActive(value);
		toggleOff.SetActive(!value);
	}

	public bool GetState()
	{
		return state;
	}
}
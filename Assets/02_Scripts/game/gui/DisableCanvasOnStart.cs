using System;
using System.Collections;
using UnityEngine;

public class DisableCanvasOnStart : MonoBehaviour
{
	public Canvas[] canvasList;
	public bool useTimer;
	public float timer;

	private void Start()
	{
		EnableImages(false);

		if (useTimer)
		{
			StartCoroutine(EnableImages());
		}
	}

	private IEnumerator EnableImages()
	{
		yield return new WaitForSeconds(timer);

		EnableImages(true);
	}

	private void EnableImages(bool enabled)
	{
		foreach (Canvas canvas in canvasList)
		{
			canvas.enabled = enabled;
		}
	}

	public void AnimationEventEnableImages(int enabled)
	{
		EnableImages(enabled != 0);
	}
}

using UnityEngine;
using UnityEngine.UI;

public class UIBlinkCanvas : BaseBehaviour
{
	public float blinkingRate;
	public float blinkingDuration;
	public AudioClip blinkingAudioClip;

	private Timer blinkingTimer;
	private Timer durationTimer;
	private float stepTime;
	private Canvas canvas;

	private void Awake()
	{
		canvas = GetComponent<Canvas>();
		blinkingTimer = new Timer(blinkingRate);
		durationTimer = new Timer(blinkingDuration);
	}

	private void Update()
	{
		durationTimer.Update(Time.deltaTime);
		if (!(durationTimer.IsExpired() && canvas.enabled))
		{
			blinkingTimer.Update(Time.deltaTime);

			if (blinkingTimer.IsExpired())
			{
				blinkingTimer.ResetElapsedTime();
				canvas.enabled = !canvas.enabled;

				if (canvas.enabled && blinkingAudioClip != null)
				{
					AudioManager.instance.PlayOneShot(blinkingAudioClip, EAudioChannelType.Sfx);
				}
			}
		}
	}

	public void Reset()
	{
		blinkingTimer.ResetElapsedTime();
		durationTimer.ResetElapsedTime();
	}
}

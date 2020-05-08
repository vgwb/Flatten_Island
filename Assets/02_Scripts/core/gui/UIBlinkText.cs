using UnityEngine;
using UnityEngine.UI;

public class UIBlinkText : BaseBehaviour
{
	public float blinkingRate;
	public AudioClip blinkingAudioClip;

	private Timer blinkingTimer;
	private float stepTime;
	private Text text;

	private void Start()
	{
		text = GetComponent<Text>();
		blinkingTimer = new Timer(blinkingRate);
	}

	private void Update()
	{
		blinkingTimer.Update(Time.deltaTime);

		if (blinkingTimer.IsExpired())
		{
			blinkingTimer.ResetElapsedTime();
			text.enabled = !text.enabled;

			if (text.enabled && blinkingAudioClip != null)
			{
				AudioManager.instance.PlayOneShot(blinkingAudioClip, EAudioChannelType.Sfx);
			}
		}
	}

	public void ResetBlinkingTimer()
	{
		blinkingTimer.ResetElapsedTime();
	}
}

using UnityEngine;
using UnityEngine.UI;

public class UIBlinkText : BaseBehaviour
{
	public float blinkingRate;

	private Timer timer;
	private float stepTime;
	private Text text;

	private void Awake()
	{
		text = GetComponent<Text>();
		timer = new Timer(blinkingRate);
	}

	private void Update()
	{
		timer.Update(Time.deltaTime);

		if (timer.IsExpired())
		{
			timer.ResetElapsedTime();
			text.enabled = !text.enabled;
		}
	}

	public void Reset()
	{
		timer.ResetElapsedTime();
	}
}

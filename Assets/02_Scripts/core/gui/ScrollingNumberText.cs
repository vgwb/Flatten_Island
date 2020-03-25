using UnityEngine;
using UnityEngine.UI;


public class ScrollingNumberText : BaseBehaviour
{
	public Text scrollingText;
	public float scrollingTime;
	public float stepValue;
	public string suffix;

	private float targetValue;
	private float currentValue;

	private Timer timer;
	private float stepTime;

	void Start()
	{
		timer = new Timer();
	}

	void Update()
	{
		if (currentValue != targetValue)
		{
			timer.Update(Time.deltaTime);

			if (timer.IsExpired())
			{
				timer.ResetElapsedTime();

				if (currentValue < targetValue)
				{
					currentValue += stepValue;
				}
				else 
				{
					currentValue -= stepValue;
				}

				scrollingText.text = currentValue + suffix;
			}
		}
	}

	public void SetTargetValue(float targetValue)
	{
		this.targetValue = targetValue;
		float difference = Mathf.Abs(currentValue - targetValue);

		timer.SetTimeout(scrollingTime / difference);
	}

	public void ForceTextValue(float value)
	{
		currentValue = value;
		targetValue = value;
		scrollingText.text = currentValue + suffix;
	}
}

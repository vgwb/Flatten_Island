using UnityEngine;

public class TimerComponent : MonoBehaviour
{
	public float minSecTimer = 0.0f;
	public float maxSecTimer = 1.0f;

	private float timer;
	private float elapsedTime;

	private void Update()
	{
		elapsedTime += Time.deltaTime;
	}

	public void ResetTimer()
	{
		timer = RandomGenerator.GetRandom(minSecTimer, maxSecTimer);
		elapsedTime = 0.0f;
	}

	public bool IsExpired()
	{
		return elapsedTime >= timer;
	}
}

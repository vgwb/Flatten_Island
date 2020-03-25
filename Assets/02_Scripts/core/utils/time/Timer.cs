public class Timer
{
	private float timeOut = 0.0f;
	private float elapsedTime = 0.0f;

	public Timer()
	{
		ResetElapsedTime();
	}

	public Timer(float timeOut) : this()
	{
		SetTimeout(timeOut);
	}

	public void SetTimeout(float timeOut)
	{
		this.timeOut = timeOut;
	}

	public void ResetElapsedTime()
	{
		elapsedTime = 0.0f;
	}

	public void Update(float deltaTime)
	{
		elapsedTime += deltaTime;
	}

	public bool IsExpired()
	{
		return (elapsedTime >= timeOut);
	}

	public float GetElapsedTime()
	{
		return elapsedTime;
	}
}
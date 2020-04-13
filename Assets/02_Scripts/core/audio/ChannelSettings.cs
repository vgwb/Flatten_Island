public class ChannelSettings
{
	private float volume;
	private bool muted;
	private bool paused;

	public ChannelSettings()
	{
		volume = 1.0f;
		muted = false;
		paused = false;
	}

	public float GetVolume()
	{
		return volume;
	}

	public void SetVolume(float volume)
	{
		this.volume = volume;
	}

	public void Mute(bool muted)
	{
		this.muted = muted;
	}

	public bool IsMuted()
	{
		return muted;
	}

	public void Pause(bool paused)
	{
		this.paused = paused;
	}

	public bool IsPaused()
	{
		return paused;
	}
}
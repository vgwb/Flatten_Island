using System.Collections.Generic;
using UnityEngine;


public class AudioChannel
{
	private List<AudioSource> audioSources;

	public ChannelSettings channelSettings;
	public EAudioChannelType audioChannelType;

	public AudioChannel(EAudioChannelType audioChannelType)
	{
		this.audioChannelType = audioChannelType;
		audioSources = new List<AudioSource>();
		channelSettings = new ChannelSettings();
	}

	public void SetMute(bool muted)
	{
		channelSettings.Mute(muted);

		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.mute = muted;
		}
	}

	public bool IsMuted()
	{
		return channelSettings.IsMuted();
	}

	public void SetPause(bool pause)
	{
		channelSettings.Pause(pause);

		foreach (AudioSource audioSource in audioSources)
		{
			if (pause)
			{
				audioSource.Pause();
			}
			else
			{
				audioSource.UnPause();
			}
		}
	}

	public bool IsPaused()
	{
		return channelSettings.IsPaused();
	}


	public void Play(AudioSource audioSource)
	{
		if (!audioSources.Contains(audioSource))
		{
			audioSources.Add(audioSource);
		}

		audioSource.volume = channelSettings.GetVolume();
		audioSource.mute = IsMuted();
		audioSource.Play();
	}

	public void Stop(AudioSource audioSource)
	{
		if (audioSources.Contains(audioSource))
		{
			audioSource.Stop();
			audioSources.Remove(audioSource);
		}
	}

	public void StopAll()
	{
		foreach (AudioSource audioSource in audioSources)
		{
			audioSource.Stop();
			audioSources.Remove(audioSource);
		}
	}
}
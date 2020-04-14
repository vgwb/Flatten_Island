using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton
{
	public AudioSource oneShotAudioSource;

	public Dictionary<EAudioChannelType, AudioChannel> channels;

	public static AudioManager instance
	{
		get
		{
			return GetInstance<AudioManager>();
		}
	}

	protected override void OnMonoSingletonStart()
	{
		base.OnMonoSingletonStart();

		channels = new Dictionary<EAudioChannelType, AudioChannel>();
		channels.Add(EAudioChannelType.Music, new AudioChannel(EAudioChannelType.Music));
		channels.Add(EAudioChannelType.Sfx, new AudioChannel(EAudioChannelType.Sfx));
	}

	public void PlayAudio(AudioSource audioSource, EAudioChannelType audioChannelType)
	{
		AudioChannel audioChannel;
		if (channels.TryGetValue(audioChannelType, out audioChannel))
		{
			audioChannel.Play(audioSource);
		}
	}

	public float GetChannelVolume(EAudioChannelType audioChannelType)
	{
		AudioChannel audioChannel;
		if (channels.TryGetValue(audioChannelType, out audioChannel))
		{
			return audioChannel.channelSettings.GetVolume();
		}

		return 1.0f;
	}

	public void SetChannelVolume(EAudioChannelType audioChannelType, float volume)
	{
		AudioChannel audioChannel;
		if (channels.TryGetValue(audioChannelType, out audioChannel))
		{
			audioChannel.channelSettings.SetVolume(volume);
		}
	}

	public void PlayOneShot(AudioClip audioClip, EAudioChannelType audioChannelType)
	{
		PlayOneShot(oneShotAudioSource, audioClip, audioChannelType);
	}

	public void PlayOneShot(AudioSource audioSource, AudioClip audioClip, EAudioChannelType audioChannelType)
	{
		AudioChannel audioChannel;
		if (!channels.TryGetValue(audioChannelType, out audioChannel))
		{
			Debug.LogError("AudioManager.PlayOneShot - audioChannel:" + audioChannelType + " not set! - clip:" + audioClip.name);
			return;
		}

		if (!audioChannel.IsMuted())
		{
			audioSource.PlayOneShot(audioClip);
		}
	}
}
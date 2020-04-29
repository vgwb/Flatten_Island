using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioManager : MonoSingleton
{
	public AudioSource oneShotAudioSource;
	public AudioSource musicAudioSource;
	public AudioClip defaultButtonClickAudioClip;

	private GameObject lastSelectedGameObject = null;

	public Dictionary<EAudioChannelType, AudioChannel> channels;

	public static AudioManager instance
	{
		get
		{
			return GetInstance<AudioManager>();
		}
	}

	protected override void OnMonoSingletonUpdate()
	{
		base.OnMonoSingletonUpdate();
		TryAddDefaultButtonSfx();
	}

	private void TryAddDefaultButtonSfx()
	{
		if (EventSystem.current != null)
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
			if (currentSelectedGameObject != null)
			{
				if (currentSelectedGameObject != lastSelectedGameObject)
				{
					lastSelectedGameObject = currentSelectedGameObject;

					Button buttonComponent = currentSelectedGameObject.GetComponent<Button>();
					if (buttonComponent != null)
					{
						PlayAudioClip playAudioClip = buttonComponent.GetComponent<PlayAudioClip>();
						if (playAudioClip == null)
						{
							playAudioClip = buttonComponent.gameObject.AddComponent<PlayAudioClip>();
							playAudioClip.audioClip = defaultButtonClickAudioClip;
							buttonComponent.onClick.AddListener(playAudioClip.Play);
						}
					}
				}
			}
		}
	}

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();

		channels = new Dictionary<EAudioChannelType, AudioChannel>();
		channels.Add(EAudioChannelType.Music, new AudioChannel(EAudioChannelType.Music));
		channels.Add(EAudioChannelType.Sfx, new AudioChannel(EAudioChannelType.Sfx));
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

	public AudioSource PlayMusic(AudioClip audioClip, float volume)
	{
		SetChannelVolume(EAudioChannelType.Music, volume);
		return PlayAudio(musicAudioSource, audioClip, EAudioChannelType.Music);
	}

	public AudioSource PlayMusic(AudioClip audioClip)
	{
		return PlayAudio(musicAudioSource, audioClip, EAudioChannelType.Music);
	}

	public AudioSource PlayAudio(AudioSource audioSource, AudioClip audioClip, EAudioChannelType audioChannelType)
	{
		AudioChannel audioChannel;
		if (!channels.TryGetValue(audioChannelType, out audioChannel))
		{
			Debug.LogError("AudioManager.PlayAudio - audioChannel:" + audioChannelType + " not set! - clip:" + audioClip.name);
			return null;
		}

		audioSource.clip = audioClip;
		audioChannel.Play(audioSource);
		return audioSource;
	}

	public void MuteChannel(EAudioChannelType audioChannelType, bool muted)
	{
		AudioChannel audioChannel;
		if (!channels.TryGetValue(audioChannelType, out audioChannel))
		{
			Debug.LogError("AudioManager.MuteChannel - audioChannel:" + audioChannelType + " not set!");
			return;
		}

		audioChannel.SetMute(muted);
	}

	public void StopMusic()
	{
		StopAudio(musicAudioSource, EAudioChannelType.Music);
	}

	public void StopMusic(AudioSource audioSource)
	{
		StopAudio(audioSource, EAudioChannelType.Music);
	}

	public void StopOneShot()
	{
		StopAudio(oneShotAudioSource, EAudioChannelType.Sfx);
	}

	public void StopOneShot(AudioSource audioSource)
	{
		StopAudio(audioSource, EAudioChannelType.Sfx);
	}

	public void StopAudio(AudioSource audioSource, EAudioChannelType audioChannelType)
	{
		AudioChannel audioChannel;
		if (!channels.TryGetValue(audioChannelType, out audioChannel))
		{
			Debug.LogError("AudioManager.StopAudio - audioChannel:" + audioChannelType + " not set!");
			return;
		}

		audioChannel.Stop(audioSource);
	}
}
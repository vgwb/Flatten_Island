using UnityEngine;
using System.Collections.Generic;

public class AudioSourceIngredient : Ingredient 
{
	public enum EAudioType
	{
		SoundFX = 0,
		Music = 1
	}
	
	private int currentIndex = 0;
	
	[SerializeField, HideInInspector]
	public EAudioType audioType = EAudioType.SoundFX;
	
	[SerializeField, HideInInspector]
	public int actionId = 0;

	[SerializeField, HideInInspector]
	public int typeId = 0;
	
	[SerializeField, HideInInspector]
	public float secondAudioEndVolume = 1.0f;
	
	[SerializeField, HideInInspector]
	public float audioDuration = 0.0f;

	private float timer = 0.0f;
	private float firstAudioStartVolume = 0.0f;

	[SerializeField, HideInInspector]
	public List<AudioSource> audioSources = new List<AudioSource>();
	
	private List<float> originalVolumes = new List<float>();

	private float settingsVolume = 1.0f;
	
	private float firstAudioStartVolumeUsed = 0.0f;
	private float secondAudioEndVolumeUsed = 0.0f;
	
	
	void Awake()
	{
		//Saving original volumes
		foreach (AudioSource a in audioSources)
		{
			if (a == null)
			{
				Debug.LogWarning("AudioSource list is not empty but an element is null! Name is:" + this.name + " - Parent:" + this.transform.parent.name);
				originalVolumes.Add(1.0f);
			}
			else
			{
				originalVolumes.Add(a.volume);
			}
		}
	}
	
	public override void Prepare()
	{
		base.Prepare();
		
		//Checking audio sources
		foreach (AudioSource audioSource in audioSources)
		{
			if (audioSource == null)
			{
				Debug.LogWarning("AudioSuorce list is not empty but an element is null! Name is:" + this.name + " - Parent:" + this.transform.parent.name);
				Abort();
				return;
			}
		}
		
		switch (audioType)
		{
			case EAudioType.Music :
				settingsVolume = 1.0f; // SaveGameManager.Instance.PlayerSlot.MusicVolume;
				break;

			case EAudioType.SoundFX :
				settingsVolume = 1.0f; //SaveGameManager.Instance.PlayerSlot.SFXVolume;
				break;
			default:
				settingsVolume = 1.0f;
				break;
		}
		
		firstAudioStartVolumeUsed = firstAudioStartVolume * settingsVolume;
		secondAudioEndVolumeUsed = secondAudioEndVolume * settingsVolume;
		
		switch (actionId)
		{
		case 0 : //play 
			switch (typeId)
			{
			case 0: //random
				ExecuteRandom();
				break;
			case 1: //sequence
				ExecuteSequence();
				break;
			case 2: //parallel
				ExecuteParallel();
				break;
			default:
				break;
			}
			break;

		case 1 : //stop 
			StopAllAudio();
			Success();
			break;

		case 2 : //Cross Fade 
			ExecuteCrossFade();
			break;

		case 3: //Fade Out
			ExecuteFadeOut();
			break;

		default:
			Debug.LogWarning("AudioSourceIngredient : Type " + actionId + " not implemented!");
			break;
		}
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);

		if (status == CookbookStatus.Running)
		{
			switch (actionId)
			{
				case 0: //play 
					switch (typeId)
					{
						case 0: //random
							UpdateRandom();
							break;
						case 1: //sequence
							UpdateSequence();
							break;
						case 2: //parallel
							UpdateParallel();
							break;
						default:
							break;
					}
					break;

				case 2: //Cross Fade
					UpdateCrossFade();
					break;

				case 3: //Fade Out
					UpdateFadeOut();
					break;

				default:
					break;
			}
		}

		return status;
	}

	public void Abort()
	{
		status = CookbookStatus.Success;
	}

	public void Success()
	{
		switch (actionId)
		{
		case 0 : //play 
			switch (typeId)
			{
			case 0: //random
				StopAllAudio(); //we stop all the audio in the sequence because we do not know which one was played.
				break;
			case 1: //sequence
				if (audioType != EAudioType.Music)
				{
					audioSources[currentIndex].Stop();
				}
				break;
			case 2: //parallel
				StopAllAudio();
				break;
			default:
				break;
			}
			break;

		case 2 : //Cross Fade
			if (audioType != EAudioType.Music)
			{
				audioSources[1].Stop();
			}
			break;

		case 3: //Fade Out
			StopAllAudio();
			break;

		default:
			break;
		}

		status = CookbookStatus.Success;
	}

	private void ExecuteSequence()
	{
		if (audioSources.Count > 0)
		{
			currentIndex = 0;
			audioSources[currentIndex].volume = originalVolumes[currentIndex] * settingsVolume;
			audioSources[currentIndex].Play();
		}
	}
	
	private void UpdateSequence()
	{
		if (!audioSources[currentIndex].isPlaying)
		{
			if (currentIndex < audioSources.Count - 1)
			{
				currentIndex++;
				audioSources[currentIndex].volume = originalVolumes[currentIndex] * settingsVolume;
				audioSources[currentIndex].Play();
			}
			else
			{
				Success();
			}
		}
		else
		{
			if (audioType == EAudioType.Music && actionId == 0 && currentIndex == audioSources.Count - 1) //if play music
			{
				Success();
			}
		}
	}

	private void ExecuteParallel()
	{
		PlayAllAudio();
	}
	
	private void UpdateParallel()
	{
		bool areAllOver = true;
		
		foreach (AudioSource i in audioSources)
		{
			if (areAllOver && (i.isPlaying))
			{
				areAllOver = false;
			}
		}
		
		if (areAllOver)
		{
			Success();
		}
	}
	
	
	private void ExecuteRandom()
	{
		if (audioSources.Count > 0)
		{
			currentIndex = Random.Range(0, audioSources.Count - 1);
			
			audioSources[currentIndex].volume = originalVolumes[currentIndex] * settingsVolume;
			audioSources[currentIndex].Play();
		}
	}

	private void UpdateRandom()
	{
		if (!audioSources[currentIndex].isPlaying)
		{
			Success();
		}
	}

	private void ExecuteCrossFade()
	{
		if (audioSources.Count == 2)
		{
			timer = 0.0f;
			
			//Gettin current time sample of first audio
			audioSources[1].volume = 0.0f;
			firstAudioStartVolumeUsed = audioSources[0].volume;
			audioSources[1].timeSamples = audioSources[0].timeSamples;
			audioSources[1].Play();
		}
		else
		{
			Debug.LogWarning("AudioSourceIngredient - Cross Fade selected but no Audio Sources! - Setting IsOver = true");
			Success();
		}
	}
	
	private void UpdateCrossFade()
	{
		timer += Time.deltaTime;
		audioSources[1].volume = (Mathf.Lerp (0.0f, secondAudioEndVolumeUsed, timer / audioDuration));
		
		float fVolume = firstAudioStartVolumeUsed - audioSources[1].volume;
		audioSources[0].volume = Mathf.Clamp01(fVolume);
		
		//Debug.Log ("Audio 0 volume:" + m_List[0].volume + " - Audio 1 volume:" + m_List[1].volume);
		if (audioSources[1].volume >= (secondAudioEndVolumeUsed))
		{
			audioSources[0].Stop();
			Success();
		}
	}

	private void ExecuteFadeOut()
	{
		if (audioSources.Count == 1)
		{
			timer = 0.0f;
			firstAudioStartVolumeUsed = audioSources[0].volume;
		}
		else
		{
			Debug.LogWarning("AudioSourceIngredient - Cross Fade selected but no Audio Source! - Setting IsOver = true");
			Success();
		}
	}

	private void UpdateFadeOut()
	{
		timer += Time.deltaTime;
		audioSources[0].volume = (Mathf.Lerp(firstAudioStartVolumeUsed, 0.0f, timer / audioDuration));

		if (audioSources[0].volume <= 0.0f)
		{
			audioSources[0].Stop();
			Success();
		}
	}

	private void StopAllAudio()
	{
		foreach (AudioSource i in audioSources)
		{
			i.Stop();
		}
	}
	
	private void PlayAllAudio()
	{
		//It might not work in mobile devices. Check it please.
		for (int i = 0; i < audioSources.Count; ++i)
		{
			audioSources[i].volume = originalVolumes[i] * settingsVolume;
			audioSources[i].Play();
		}
	}
	
	private void PauseAllAudio()
	{
		foreach (AudioSource i in audioSources)
		{
			if (i.isPlaying)
			{
				i.Pause();
			}
		}
	}
}

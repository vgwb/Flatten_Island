using UnityEngine;

public class PlayAudioClip : MonoBehaviour
{
	public AudioClip audioClip;

	public void Play()
	{
		if (audioClip != null)
		{
			AudioManager.instance.PlayOneShot(audioClip, EAudioChannelType.Sfx);
		}
	}
}
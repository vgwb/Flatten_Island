using UnityEngine;

public class PlayRandomAudioClipIngredient : Ingredient
{
	public AudioClip[] audioClips;

	private ShuffleBag<AudioClip> shuffleBag;

	private void Awake()
	{
		shuffleBag = new ShuffleBag<AudioClip>(3);
		foreach (AudioClip audioClip in audioClips)
		{
			shuffleBag.Add(audioClip, 1);
		}

		shuffleBag.Shuffle();
	}

	public override CookbookStatus Use(float deltaTime)
	{
		base.Use(deltaTime);
		if (status == CookbookStatus.Running)
		{
			AudioClip audioClip = shuffleBag.Next();
			AudioManager.instance.PlayOneShot(audioClip, EAudioChannelType.Sfx);
			status = CookbookStatus.Success;
		}

		return status;
	}
}

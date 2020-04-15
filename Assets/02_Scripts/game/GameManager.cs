using UnityEngine;
using Messages;

public class GameManager : MonoSingleton
{
	public Camera mainCamera { get { return Camera.main; } }

	public Platform platform;
	public SuggestionFactory suggestionFactory;
	public LocalPlayer localPlayer;
	public float musicChannelVolume = 0.2f;
	public float sfxChannelVolume = 1.0f;

	public ISaveGameSerializer gameSerializer
	{
		get; private set;
	}

	public ISaveGameStorage saveGameStorage;

	public static GameManager instance
	{
		get
		{
			return GetInstance<GameManager>();
		}
	}

	protected override void OnMonoSingletonStart()
	{
		base.OnMonoSingletonStart();

		saveGameStorage = new LocalSaveGameStorage();

		gameSerializer = new JsonGameSerializer();

		suggestionFactory = new SuggestionFactory();

		PlatformCreator platformCreator = new PlatformCreator();
		platform = platformCreator.CreatePlatform();

		AudioManager.instance.SetChannelVolume(EAudioChannelType.Music, musicChannelVolume);
		AudioManager.instance.SetChannelVolume(EAudioChannelType.Sfx, sfxChannelVolume);

		LocalizationManager.instance.Init();
		SetLanguage();
		Debug.Log("Current Language:" + LocalizationManager.instance.GetCurrentLanguage());

		LoadPlayer();
	}

	protected override void OnMonoSingletonUpdate()
	{
		EventMessageManager.instance.Update();
	}

	public void LoadPlayer()
	{
		localPlayer = gameSerializer.ReadSaveGame(saveGameStorage) as LocalPlayer;
		if (localPlayer == null)
		{
			localPlayer = new LocalPlayer();
			localPlayer.Init();
		}
	}

	public void SavePlayer()
	{
		gameSerializer.WriteSaveGame(saveGameStorage, localPlayer);
	}

	public bool HasPendingGameSession()
	{
		return localPlayer.HasSession();
	}

	private void SetLanguage()
	{
		if (!LocalizationManager.instance.HasCurrentLanguage())
		{
			string languageCode = platform.GetCurrentLanguage();
			if (languageCode != null)
			{
				LocalizationXmlModel localizationXmlModel = LocalizationManager.instance.FindLocalizationXmlModel(languageCode);
				if (localizationXmlModel != null)
				{
					LocalizationManager.instance.SetLanguage(localizationXmlModel.languageId);
				}
			}
			else
			{
				LocalizationManager.instance.SetDefaultLanguage();
			}
		}
	}
}
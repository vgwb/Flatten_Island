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
		Debug.Log("GameManager - SetLanguage()");

		if (!LocalizationManager.instance.HasCurrentLanguage())
		{
			Debug.Log("GameManager - SetLanguage() - No current language found");
			string languageCode = platform.GetCurrentLanguage();
			Debug.Log("GameManager - SetLanguage() - platform language code" + languageCode);
			if (languageCode != null)
			{
				LocalizationXmlModel localizationXmlModel = LocalizationManager.instance.FindLocalizationXmlModel(languageCode);
				Debug.Log("GameManager - SetLanguage() - localizationXmlModel:" + localizationXmlModel);
				if (localizationXmlModel != null)
				{
					Debug.Log("GameManager - SetLanguage() - Setting language:" + localizationXmlModel.languageId);
					LocalizationManager.instance.SetLanguage(localizationXmlModel.languageId);
				}
			}
			else
			{
				Debug.Log("GameManager - SetLanguage() - Setting Default Language");
				LocalizationManager.instance.SetDefaultLanguage();
			}
		}
	}
}
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

		LoadPlayer();

		SetLanguage();
		Debug.Log("Current Language:" + localPlayer.GetLanguageId());
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

	public bool TryUpdateHighScore()
	{
		bool newDayHighScore = localPlayer.TryUpdateDayHighScore();
		bool newGrowthRateHighScore = localPlayer.TryUpdateGrowthRateHighScore();
		bool newPublicOpinionHighScore = localPlayer.TryUpdatePublicOpinionHighScore();

		if (newDayHighScore || newGrowthRateHighScore || newPublicOpinionHighScore )
		{
			SavePlayer();
			return true;
		}

		return false;
	}

	private void SetLanguage()
	{
		if (!localPlayer.HasLanguageId())
		{
			string languageCode = platform.GetCurrentLanguage();
			if (languageCode != null)
			{
				LocalizationXmlModel localizationXmlModel = LocalizationManager.instance.FindLocalizationXmlModel(languageCode);
				if (localizationXmlModel != null)
				{
					localPlayer.SetLanguageId(localizationXmlModel.languageId);
				}
			}
			else
			{
				localPlayer.SetLanguageId(LocalizationManager.DEFAULT_LANGUAGE_ID);
			}
		}
	}
}
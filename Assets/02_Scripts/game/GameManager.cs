using UnityEngine;
using Messages;

public class GameManager : MonoSingleton
{
	public Camera mainCamera { get { return Camera.main; } }

	public Platform platform;
	public LocalPlayer localPlayer;

	public GameSerializer gameSerializer
	{
		get; private set;
	}

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

		gameSerializer = new PlayerPrefsGameSerializer();

		localPlayer = new LocalPlayer();

		PlatformCreator platformCreator = new PlatformCreator();
		platform = platformCreator.CreatePlatform();

		LocalizationManager.instance.Init();
		SetLanguage();

		Debug.Log("Current Language:" + LocalizationManager.instance.GetCurrentLanguage());
	}

	protected override void OnMonoSingletonUpdate()
	{
		EventMessageManager.instance.Update();
	}

	protected override void OnMonoSingletonDestroyed()
	{
		localPlayer = null;
		base.OnMonoSingletonDestroyed();
	}

	public void LoadLocalPlayer()
	{
		gameSerializer.ReadSaveGame(out localPlayer);
	}

	public void SaveLocalPlayer()
	{
		gameSerializer.WriteSaveGame(localPlayer);
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
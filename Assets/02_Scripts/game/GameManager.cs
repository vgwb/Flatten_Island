﻿using UnityEngine;
using Messages;

public class GameManager : MonoSingleton
{
	public Camera mainCamera { get { return Camera.main; } }

	public Platform platform;
	public SuggestionFactory suggestionFactory;
	public LocalPlayer localPlayer;

	public GameSerializer gameSerializer
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

	protected override void OnMonoSingletonAwake()
	{
		base.OnMonoSingletonAwake();

		EventMessageHandler suggestionResultEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionResultEntryExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, suggestionResultEntryExitCompletedMessageHandler);
	}

	protected override void OnMonoSingletonStart()
	{
		base.OnMonoSingletonStart();

		saveGameStorage = new LocalSaveGameStorage();

		gameSerializer = new JsonGameSerializer();

		suggestionFactory = new SuggestionFactory();

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

		EventMessageManager.instance.RemoveHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, this);

		base.OnMonoSingletonDestroyed();
	}

	public void LoadPlayer()
	{
		gameSerializer.ReadSaveGame(saveGameStorage, out localPlayer);
	}

	public void SavePlayer()
	{
		gameSerializer.WriteSaveGame(saveGameStorage, localPlayer);
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

	private void OnSuggestionResultEntryExitCompleted(EventMessage eventMessage)
	{
		SuggestionResultEntryExitCompletedEvent suggestionResultEntryExitCompletedEvent = eventMessage.eventObject as SuggestionResultEntryExitCompletedEvent;

		SuggestionOptionXmlModel selectedSuggestionOptionXmlModel = suggestionResultEntryExitCompletedEvent.selectedSuggestionOptionXmlModel;

		localPlayer.ApplySuggestionOption(selectedSuggestionOptionXmlModel);

		//Should be in the Game Manager
		MainScene.instance.StartDayTransition();
		MainScene.instance.AnimateDayTransition(); // TODO probably not instantaneous

		GameManager.instance.SavePlayer(); //temp

		AdvisorsManager.instance.ShowAdvisors();
	}
}
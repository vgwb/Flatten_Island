using Messages;

public class GameSessionFsm : FiniteStateMachine
{
	public static readonly string DayStartState = "DayStartState";
	public static readonly string ChangeGamePhaseState = "ChangeGamePhaseState";
	public static readonly string AdvisorsState = "AdvisorsState";
	public static readonly string SuggestionState = "SuggestionState";
	public static readonly string SuggestionResultState = "SuggestionResultState";
	public static readonly string UpdateResultState = "UpdateResultState";
	public static readonly string NextDayConfirmationState = "NextDayConfirmationState";
	public static readonly string WinningState = "WinningState";
	public static readonly string LosingState = "LosingState";

	private GameSession gameSession;

	private SuggestionOptionXmlModel selectedSuggestionOption;

	public GameSessionFsm(GameSession gameSession) : base()
    {
		this.gameSession = gameSession;
    }

	public void Dispose()
	{
		EventMessageManager.instance.RemoveHandler(typeof(GenericDialogExitCompletedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(AllAdvisorsExitCompletedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionEntryExitCompletedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, this);
		EventMessageManager.instance.RemoveHandler(typeof(NextDayEntryExitCompletedEvent).Name, this);
	}

	protected override void Initialize()
	{
		base.Initialize();
		AddState(DayStartState, DayStart_Enter, DayStart_Update, null);
		AddState(ChangeGamePhaseState, ChangeGamePhase_Enter, null, ChangeGamePhase_Exit);
		AddState(AdvisorsState, Advisors_Enter, null, Advisors_Exit);
		AddState(SuggestionState, Suggestion_Enter, null, Suggestion_Exit);
		AddState(SuggestionResultState, SuggestionResult_Enter, null, SuggestionResult_Exit);
		AddState(UpdateResultState, UpdateResult_Enter, UpdateResult_Update, null);
		AddState(NextDayConfirmationState, NextDayConfirmation_Enter, null, NextDayConfirmation_Exit);
		AddState(WinningState, Winning_Enter, null, Winning_Exit);
		AddState(LosingState, Losing_Enter, null, Losing_Exit);
	}

	public void StartFsm()
	{
		Start(DayStartState);
	}


	private void DayStart_Enter()
	{
		Hud.instance.UpdateDayValues();
		GameManager.instance.SavePlayer();
	}

	private void DayStart_Update()
	{
		if (gameSession.HasPlayerWon())
		{
			TriggerState(WinningState);
			return;
		}

		if (gameSession.HasPlayerLose())
		{
			TriggerState(LosingState);
			return;
		}

		if (gameSession.IsCurrentPhaseFinished())
		{
			TriggerState(ChangeGamePhaseState);
			return;
		}

		TriggerState(AdvisorsState);
	}

	private void Winning_Enter()
	{
		GameManager.instance.TryUpdateHighScore();

		EventMessageHandler winningDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnWinningDialogExit);
		EventMessageManager.instance.AddHandler(typeof(GenericDialogExitCompletedEvent).Name, winningDialogExitCompletedMessageHandler);
		GenericDialog.Show(5003, MainScene.instance.uiWorldCanvas.transform);
	}

	private void OnWinningDialogExit(EventMessage eventMessage)
	{
		//TO DO, before quitting should record the high score
		GameManager.instance.localPlayer.QuitGameSession();
		ScenesFlowManager.instance.UnloadingMainScene();
	}

	private void Winning_Exit()
	{
		EventMessageManager.instance.RemoveHandler(typeof(GenericDialogExitCompletedEvent).Name, this);
	}

	private void Losing_Enter()
	{
		GameManager.instance.TryUpdateHighScore();

		EventMessageHandler losingDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnLosingDialogExit);
		EventMessageManager.instance.AddHandler(typeof(GenericDialogExitCompletedEvent).Name, losingDialogExitCompletedMessageHandler);
		GenericDialog.Show(5004, MainScene.instance.uiWorldCanvas.transform);
	}

	private void OnLosingDialogExit(EventMessage eventMessage)
	{
		//TO DO, before quitting should record the high score if there is one
		GameManager.instance.localPlayer.QuitGameSession();
		GameManager.instance.SavePlayer();
		ScenesFlowManager.instance.UnloadingMainScene();
	}

	private void Losing_Exit()
	{
		EventMessageManager.instance.RemoveHandler(typeof(GenericDialogExitCompletedEvent).Name, this);
	}

	private void ChangeGamePhase_Enter()
	{
		int nextPhaseId = gameSession.gamePhase.GetNextPhaseId();
		gameSession.gamePhase.Stop();
		gameSession.StartGamePhase(nextPhaseId);

		//just to test the flow
		EventMessageHandler changeGamePhaseDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnChangePhaseEntryExit);
		EventMessageManager.instance.AddHandler(typeof(GenericDialogExitCompletedEvent).Name, changeGamePhaseDialogExitCompletedMessageHandler);
		GenericDialog.Show(5002, MainScene.instance.uiWorldCanvas.transform);
	}

	private void OnChangePhaseEntryExit(EventMessage eventMessage)
	{
		TriggerState(AdvisorsState);
	}

	private void ChangeGamePhase_Exit()
	{
		EventMessageManager.instance.RemoveHandler(typeof(GenericDialogExitCompletedEvent).Name, this);
	}

	private void Advisors_Enter()
	{
		EventMessageHandler allAdvisorsExitCompletedMessageHandler = new EventMessageHandler(this, OnAllAdvisorsExitCompletedEvent);
		EventMessageManager.instance.AddHandler(typeof(AllAdvisorsExitCompletedEvent).Name, allAdvisorsExitCompletedMessageHandler);

		gameSession.advisors = AdvisorsManager.instance.PickAdvisors();
		GameManager.instance.SavePlayer();

		AdvisorsManager.instance.ShowAdvisors(gameSession.advisors);
	}

	private void OnAllAdvisorsExitCompletedEvent(EventMessage eventMessage)
	{
		AllAdvisorsExitCompletedEvent allAdvisorsExitCompletedEvent = eventMessage.eventObject as AllAdvisorsExitCompletedEvent;
		TriggerState(SuggestionState);
	}

	private void Advisors_Exit()
	{
		EventMessageManager.instance.RemoveHandler(typeof(AllAdvisorsExitCompletedEvent).Name, this);
	}

	private void Suggestion_Enter()
	{
		EventMessageHandler suggestionEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionExitCompletedEvent);
		EventMessageManager.instance.AddHandler(typeof(SuggestionEntryExitCompletedEvent).Name, suggestionEntryExitCompletedMessageHandler);

		AdvisorsManager.instance.ShowAdvisorSuggestion();
	}

	private void OnSuggestionExitCompletedEvent(EventMessage eventMessage)
	{
		SuggestionEntryExitCompletedEvent suggestionEntryExitCompletedEvent = eventMessage.eventObject as SuggestionEntryExitCompletedEvent;
		selectedSuggestionOption = suggestionEntryExitCompletedEvent.selectedSuggestionOption;

		TriggerState(SuggestionResultState);
	}

	private void Suggestion_Exit()
	{
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionEntryExitCompletedEvent).Name, this);
	}

	private void SuggestionResult_Enter()
	{
		EventMessageHandler suggestionResultEntryExitCompletedMessageHandler = new EventMessageHandler(this, OnSuggestionResultExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, suggestionResultEntryExitCompletedMessageHandler);

		AdvisorsManager.instance.ShowAdvisorSuggestionResult(selectedSuggestionOption);
	}

	private void OnSuggestionResultExitCompleted(EventMessage eventMessage)
	{
		TriggerState(UpdateResultState);
	}

	private void SuggestionResult_Exit()
	{
		EventMessageManager.instance.RemoveHandler(typeof(SuggestionResultEntryExitCompletedEvent).Name, this);
	}

	private void UpdateResult_Enter()
	{
		gameSession.ApplySuggestionOption(selectedSuggestionOption);
		Hud.instance.UpdateSuggestionOptions();
		ChartManager.instance.RestartChartAnimation();
	}

	private void UpdateResult_Update()
	{
		//should wait here for the chart animation to finish
		TriggerState(NextDayConfirmationState);
	}

	private void NextDayConfirmation_Enter()
	{
		EventMessageHandler nextDayDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnNextDayDialogExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(NextDayEntryExitCompletedEvent).Name, nextDayDialogExitCompletedMessageHandler);

		gameSession.ShowNextDayEntry();
	}

	private void OnNextDayDialogExitCompleted(EventMessage eventMessage)
	{
		NextDayEntryExitCompletedEvent nextDayExitCompletedEvent = eventMessage.eventObject as NextDayEntryExitCompletedEvent;
		TriggerState(DayStartState);
	}

	private void NextDayConfirmation_Exit()
	{
		EventMessageManager.instance.RemoveHandler(typeof(NextDayEntryExitCompletedEvent).Name, this);
		gameSession.NextDay();
	}
}

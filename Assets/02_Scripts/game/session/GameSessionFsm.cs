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
		EventMessageManager.instance.RemoveHandler(typeof(GameOverEntryExitCompletedEvent).Name, this);
	}

	protected override void Initialize()
	{
		base.Initialize();
		AddState(DayStartState, DayStart_Enter, DayStart_Update, null);
		AddState(ChangeGamePhaseState, ChangeGamePhase_Enter, ChangeGamePhase_Update, ChangeGamePhase_Exit);
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
		gameSession.gamePhase.DayStart_Enter();
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
		EventMessageManager.instance.AddHandler(typeof(GameOverEntryExitCompletedEvent).Name, winningDialogExitCompletedMessageHandler);
		gameSession.ShowWonDialog();
	}

	private void OnWinningDialogExit(EventMessage eventMessage)
	{
		GameManager.instance.localPlayer.QuitGameSession();
		ScenesFlowManager.instance.UnloadingMainScene();
	}

	private void Winning_Exit()
	{
		EventMessageManager.instance.RemoveHandler(typeof(GameOverEntryExitCompletedEvent).Name, this);
	}

	private void Losing_Enter()
	{
		EventMessageHandler losingDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnLosingDialogExit);
		EventMessageManager.instance.AddHandler(typeof(GameOverEntryExitCompletedEvent).Name, losingDialogExitCompletedMessageHandler);
		gameSession.ShowLoseDialog();
	}

	private void OnLosingDialogExit(EventMessage eventMessage)
	{
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
		gameSession.gamePhase.ChangeGamePhase_Enter();
	}

	private void ChangeGamePhase_Update()
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

		gameSession.gamePhase.Advisor_Enter();
	}

	private void OnAllAdvisorsExitCompletedEvent(EventMessage eventMessage)
	{
		AllAdvisorsExitCompletedEvent allAdvisorsExitCompletedEvent = eventMessage.eventObject as AllAdvisorsExitCompletedEvent;
		TriggerState(SuggestionState);
	}

	private void Advisors_Exit()
	{
		gameSession.DiscardAdvisors();
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

		gameSession.gamePhase.UpdateResult_Enter();
	}

	private void UpdateResult_Update()
	{
		gameSession.gamePhase.UpdateResult_Update(this);
	}

	private void NextDayConfirmation_Enter()
	{
		EventMessageHandler nextDayDialogExitCompletedMessageHandler = new EventMessageHandler(this, OnNextDayDialogExitCompleted);
		EventMessageManager.instance.AddHandler(typeof(NextDayEntryExitCompletedEvent).Name, nextDayDialogExitCompletedMessageHandler);

		gameSession.gamePhase.NextDayConfirmation_Enter();
	}

	private void OnNextDayDialogExitCompleted(EventMessage eventMessage)
	{
		NextDayEntryExitCompletedEvent nextDayExitCompletedEvent = eventMessage.eventObject as NextDayEntryExitCompletedEvent;
		TriggerState(DayStartState);
	}

	private void NextDayConfirmation_Exit()
	{
		EventMessageManager.instance.RemoveHandler(typeof(NextDayEntryExitCompletedEvent).Name, this);
		gameSession.gamePhase.NextDayConfirmation_Exit();
	}
}

using System;

public class LoadingPanelFsm : FiniteStateMachine
{
	public static readonly string InactiveState = "Inactive";
	public static readonly string EnteringState = "Entering";
	public static readonly string ShownState = "Shown";
	public static readonly string ExitingState = "Exiting";

	private LoadingPanel loadingPanel;

    public LoadingPanelFsm(LoadingPanel loadingPanel) : base()
    {
		this.loadingPanel = loadingPanel;
    }

	protected override void Initialize()
	{
		base.Initialize();
		AddState(InactiveState, null, null, null);
		AddState(EnteringState, Entering_Enter, null, Entering_Exit);
		AddState(ShownState, null, null, null);
		AddState(ExitingState, Exiting_Enter, null, Exiting_Exit);
	}

	public void StartFsm()
	{
		Start(InactiveState);
	}

	private void Entering_Enter()
	{
		loadingPanel.loadingPanelChef.Cook(loadingPanel.loadingPanelChef.enterRecipe, OnEnteringCompleted);
	}

	private void OnEnteringCompleted()
	{
		TriggerState(ShownState);
	}

	private void Entering_Exit()
	{
		if (loadingPanel.onEnteredEvent != null)
		{
			loadingPanel.onEnteredEvent();
			loadingPanel.onEnteredEvent = null;
		}
	}

	private void Exiting_Enter()
	{
		loadingPanel.loadingPanelChef.Cook(loadingPanel.loadingPanelChef.exitRecipe, OnExitingCompleted);
	}

	private void OnExitingCompleted()
	{
		TriggerState(InactiveState);
	}

	private void Exiting_Exit()
	{
		//StopLoadingAnimation
		if (loadingPanel.onExitedEvent != null)
		{
			loadingPanel.onExitedEvent();
			loadingPanel.onExitedEvent = null;
		}
	}
}

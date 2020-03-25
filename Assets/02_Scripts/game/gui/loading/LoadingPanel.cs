using UnityEngine;
using System;

public class LoadingPanel : MonoBehaviour
{
	public LoadingPanelChef loadingPanelChef;

	public Action onEnteredEvent;
    public Action onExitedEvent;

	private LoadingPanelFsm loadingPanelFsm;

    void Start()
    {
		loadingPanelFsm = new LoadingPanelFsm(this);
		loadingPanelFsm.StartFsm();
    }

	void Update()
	{
		loadingPanelFsm.Update();
	}

    public void Enter(Action onEnter = null)
	{
		onEnteredEvent += onEnter;
		loadingPanelFsm.TriggerState(LoadingPanelFsm.EnteringState);
	}

    public void Exit(Action onExit = null)
	{
		onExitedEvent += onExit;
		loadingPanelFsm.TriggerState(LoadingPanelFsm.ExitingState);
	}

	public bool IsEnteringAnimationFinished() { return true; }
	public bool IsExitingAnimationFinished() { return true; }
}

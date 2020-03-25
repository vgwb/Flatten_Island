using System;

public class FsmState
{
    public string name;

	public Action onEnter;
	public Action onUpdate;
	public Action onExit;

	public FsmState(string name, Action onEnter, Action onUpdate, Action onExit)
	{
		this.name = name;
		this.onEnter = onEnter;
		this.onUpdate = onUpdate;
		this.onExit = onExit;
	}

	public void OnEnter()
	{
		if (onEnter != null)
		{
			onEnter();
		}
	}

	public void OnUpdate()
	{
		if (onUpdate != null)
		{
			onUpdate();
		}
	}

	public void OnExit()
	{
		if (onExit != null)
		{
			onExit();
		}
	}
}
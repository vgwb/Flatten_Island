using System;
using System.Collections.Generic;

public class FiniteStateMachine
{
    private Dictionary<string, FsmState> states;
    private FsmState currentState;
 
	protected virtual void Initialize()
	{
		states = new Dictionary<string, FsmState>();
		currentState = null;
	}

	protected void AddState(string name, Action onEnter, Action onUpdate, Action onExit)
	{
		FsmState newState = new FsmState(name, onEnter, onUpdate, onExit);
		states.Add(newState.name, newState);
	}

	protected void Start(string initialState)
	{
		Initialize();
		TriggerState(initialState);
	}

	public virtual void TriggerState(string nextStateName)
	{
		if ((currentState != null) && (currentState.name != nextStateName))
		{
			currentState.OnExit();
		}

		currentState = states[nextStateName];
		currentState.OnEnter();
	}

	public bool IsInState(string stateName)
	{
		if ((currentState != null) && (currentState.name != null))
		{
			return currentState.name.Equals(stateName);
		}

		return false;
	}

	public string GetCurrentState()
	{
		if (currentState != null)
		{
			return currentState.name;
		}

		return "";
	}

	public void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }
}
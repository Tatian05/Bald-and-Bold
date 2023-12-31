using System.Collections.Generic;
using UnityEngine;
public enum StateName
{
    Climb,               //Player
}

public class StateMachine : MonoBehaviour
{
    public IState currentState;

    private Dictionary<StateName, IState> allStates = new Dictionary<StateName, IState>();

    public void Update()
    {
        if (currentState != null) currentState.OnUpdate();
    }
    public void FixedUpdate()
    {
        if (currentState != null) currentState.OnFixedUpdate();
    }
    public void AddState(StateName key, IState state)
    {
        if (!allStates.ContainsKey(key)) allStates.Add(key, state);
    }
    public void ChangeState(StateName key)
    {
        if (!allStates.ContainsKey(key)) return;

        if (currentState != null) currentState.OnExit();
        currentState = allStates[key];
        currentState.OnEnter();
    }

    public bool IsInState(StateName key)
    {
        return currentState == allStates[key] ? true : false ;
    }
}

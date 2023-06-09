using UnityEngine;
using System.Collections.Generic;
using System;

public class UnitStateController : MonoBehaviour
{
    // Internal dictionary of type of state for the key and the instance of it for the value
    private Dictionary<Type, BaseState> activeStates = new Dictionary<Type, BaseState>();

    [SerializeField, Tooltip("List of all state behaviors on this unit")]
    private List<BaseState> activeStateList;

    private void Awake()
    {
        // Find all BaseState-derived components attached to the unit
        BaseState[] states = GetComponents<BaseState>();

        // Add the states to the activeStates dictionary
        foreach (BaseState state in states)
        {
            AddState(state);
        }

        UpdateStateList();
    }

    private void UpdateStateList()
    {
        /*
        FOR DEVELOPING PURPOSES ONLY
        DISPLAYS LIST OF ALL ACTIVE STATES
         */

        activeStateList = new List<BaseState>(activeStates.Values);
    }

    public void AddState<StateType>(StateType newState) where StateType : BaseState
    {
        Type stateType = typeof(StateType);

        if (!activeStates.ContainsKey(stateType))
        {
            activeStates[stateType] = newState;
            newState.EnterState();
        }

        UpdateStateList();
    }

    public void RemoveState<StateType>() where StateType : BaseState
    {
        Type stateType = typeof(StateType);

        // Check if the state is active on this unit
        if (activeStates.ContainsKey(stateType))
        {
            BaseState stateToRemove = activeStates[stateType];
            //stateToRemove.ExitState();
            activeStates.Remove(stateType);
        }

        UpdateStateList();
    }

    private void Update()
    {
        // Check if this unit is idle or not & this is not focusing on anything
        if (activeStates.ContainsKey(typeof(IdleState)) && gameObject.GetComponent<Unit>().focus == null)
        {
            PatrolState patrolState = new PatrolState();
            AddState(patrolState);
        }
    }
}

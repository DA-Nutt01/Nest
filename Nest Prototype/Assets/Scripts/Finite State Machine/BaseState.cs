using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(UnitStateController))]
public abstract class BaseState : MonoBehaviour
{
    [Header("Base State Congifigurations"), Space(10)]
    [SerializeField, Tooltip("Reference to the unit controlled by this state")]
    protected Unit unit;

    
    [SerializeField, Tooltip("Reference to UnitStateController Component on this")]
    protected UnitStateController stateController;


    public virtual void Awake()
    {
        unit = GetComponent<Unit>();
        stateController = GetComponent<UnitStateController>();
    }

    public virtual void EnterState()
    {
    }

    public virtual void ExitState()
    {
    }

    // Implementation for the state behavior
    public virtual IEnumerator UpdateState()
    {
        yield break;
    }
}

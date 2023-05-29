using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public abstract class BaseState : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the unit controlled by this state")]
    protected Unit unit;

    public virtual void Awake()
    {
        unit = GetComponent<Unit>();
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

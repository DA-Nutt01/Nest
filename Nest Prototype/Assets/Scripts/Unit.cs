using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    #region Global Variables
    public NavMeshAgent agent;
    #endregion

    void Start()
    {
        // Cache a ref to this unit's nav mesh agent component
        agent = GetComponent<NavMeshAgent>();
        // Add this unit to list of all units in the game
        UnitSelectionManager.Instance.allUnits.Add(this.gameObject);
    }

    void OnDestroy() 
    {
        // When destroyed, remove this unit from the list of all units
        UnitSelectionManager.Instance.allUnits.Remove(this.gameObject);
    }

    public void Move(Vector3 point)
    {
        agent.SetDestination(point);
    }
}

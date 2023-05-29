using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolState : BaseState
{
    [SerializeField, Tooltip("The positions the unit will circulate between")]
    private List<Vector3> patrolPoints;

    [SerializeField, Tooltip("The index of the current position the unit is traveling to")]
    private int currentPointIndex;

    public override void Awake()
    {
        // Hardcoding some random patrol points for now
        patrolPoints.Add(new Vector3(Random.Range(-125, 125), gameObject.transform.position.y, Random.Range(-125, 125)));
        patrolPoints.Add(new Vector3(Random.Range(-125, 125), gameObject.transform.position.y, Random.Range(-125, 125)));
        currentPointIndex = 0;
        base.Awake();
    }

    public override void EnterState()
    {
        Debug.Log("Entering Patrol State");
        
        StartCoroutine(UpdateState());
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Patrol State");
    }

    public override IEnumerator UpdateState()
    {
        Debug.Log("Patrolling...");

        while (true)
        {
            if (unit.agent != null)
            {
                // Move towards the current patrol point
                Vector3 targetPosition = patrolPoints.ToArray()[currentPointIndex];
                unit.Move(targetPosition, unit.patrolSpeed);

                // Check if the unit has reached the patrol point
                 if (Vector3.Distance(unit.transform.position, targetPosition) < 0.1f)
                 {
                     // Move to the next patrol point
                     currentPointIndex++;
                     if (currentPointIndex >= patrolPoints.Count)
                         currentPointIndex = 0; 
                 }
            }
            yield return null;
        }
    }
}

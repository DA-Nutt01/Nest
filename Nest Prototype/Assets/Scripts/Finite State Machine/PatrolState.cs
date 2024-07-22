using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PatrolState : BaseState
{
    [Space(20), Header("Patrol State Configurations"), Space(10)]

    [SerializeField, Tooltip("The index of the current position the unit is traveling to")]
    private int currentPointIndex;

    [SerializeField, Tooltip("Layer Mask to filter enemy units")]
    private LayerMask enemyMask;

    [SerializeField, Tooltip("Reference to NavMeshAgent component on this")]
    private NavMeshAgent agent;

    [Space(10), SerializeField, Tooltip("The positions the unit will circulate between")]
    private List<Vector3> patrolPoints;

    public override void Awake()
    {
        base.Awake();
        // Hardcoding some random patrol points for now
        patrolPoints.Add(new Vector3(Random.Range(0, 20), gameObject.transform.position.y, Random.Range(0, 20)));
        patrolPoints.Add(new Vector3(Random.Range(0, 20), gameObject.transform.position.y, Random.Range(0, 20)));
        currentPointIndex = 0;
        agent = unit.agent;
        //agent.stoppingDistance = 0f;

        // Check what type of unit this is to determine what layers define as enemy layers
        switch (unit.unitType)
        {
            case (UnitType.Human):
                enemyMask = LayerMask.GetMask("Alien Unit", "Alien Structure");
                break;
            case (UnitType.Alien):
                enemyMask = LayerMask.GetMask("Human Unit", "Human Structure");
                break;
        }
    }

    public override void EnterState()
    {        
        StartCoroutine(UpdateState());
    }

    public override void ExitState()
    {
        //unit.agent.stoppingDistance = 1.5f;
        StopAllCoroutines();
        //stateController.RemoveState <PatrolState>();
    }

    public override IEnumerator UpdateState()
    {
        while (true)
        {
            if (agent != null)
            {
                // Constantly cache any player unit enters this unit's detection radius
                Collider[] detectedPlayerUnits = Physics.OverlapSphere(transform.position, unit.detectionRadius, enemyMask);

                // Move towards the current patrol point
                Vector3 targetPosition = patrolPoints.ToArray()[currentPointIndex];
                unit.Move(targetPosition, unit.patrolSpeed);

                // Check if the unit has reached the patrol point
                 if (Vector3.Distance(unit.transform.position, targetPosition) < 0.1f)
                 {
                    // Wait a few moments before moving to next patrol point
                    yield return new WaitForSeconds(5);

                    // Move to the next patrol point
                     currentPointIndex++;
                     if (currentPointIndex >= patrolPoints.Count) 
                     {
                        currentPointIndex = 0; 
                     }
                 }
                 
                 if (detectedPlayerUnits.Length >= 1 && unit.focus == null)
                {
                    unit.SetFocus(detectedPlayerUnits[0].GetComponent<Interactable>());
                    yield return StartCoroutine(unit.FollowTarget());
                    unit.Interact();
                    //ExitState();
                    //Destroy(this);
                }
            }
            yield return null;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue; // Set Gizmo color to red
        foreach (Vector3 patrolPoint in patrolPoints)
        {
            Gizmos.DrawWireSphere(patrolPoint, 1f); // Draw wire sphere at the unit's position with the detection radius
        }
        
    }
}

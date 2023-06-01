using UnityEngine;
using BahaviorTree;

public class TaskPatrol : Node
{   
    [Tooltip("Unit component of this unit")]
    public Unit _unit;
    [Tooltip("Array of transforms this unit will patrol to")]
    public Transform[] _waypoints;

    [SerializeField, Tooltip("Array index of current waypoint this unit is traveling to")]
    private int   _currentWaypointIndex = 0;

    [SerializeField, Tooltip("Amount of seconds this unit will wait between traveling to waypoints")]
    private float _waitTime = 1f; 
    [SerializeField, Tooltip("Amount of time in seconds this unit has been waiting")]
    private float _waitCounter = 0f;
    [SerializeField, Tooltip("If this unit is currently waiting or not")]
    private bool  _isWaiting = false;

    public void Initialize(Transform unitTransform, Transform[] waypoints)
    {
        _unit = GetComponent<Unit>();
        _waypoints = waypoints;
        _unit.agent.stoppingDistance = 0f; // Must reset stopping distance when no longer patroling
    }

    public override NodeState EvaluateState()
    {
         if (_isWaiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime) _isWaiting = false;
        }
        else
        {
            Transform waypoint = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(_unit.transform.position, waypoint.position) < 0.1f)
            {
                Debug.Log("REACHED WAYPOINT");
                _waitCounter = 0f;
                _isWaiting = true;

                if (_currentWaypointIndex == _waypoints.Length - 1) _currentWaypointIndex = 0; // Reset index to 0
                else _currentWaypointIndex++; // Otherwise increment index by 1
            }
            else
            {
                _unit.MoveTo(waypoint.position, _unit.patrolSpeed);
            }
        }

        nodeState = NodeState.Running;
        return nodeState;
    }
}

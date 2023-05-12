using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    #region Global Variables
    public NavMeshAgent agent;
    public Interactable focus;
    #endregion

    void Start()
    {
        // Cache a ref to this unit's nav mesh agent component
        agent = GetComponent<NavMeshAgent>();
        // Add this unit to list of all units in the game
        UnitSelectionManager.Instance.allUnits.Add(this.gameObject);
        // Set focus to none
        Defocus();
    }

    void OnDestroy() 
    {
        // When destroyed, remove this unit from the list of all units
        UnitSelectionManager.Instance.allUnits.Remove(this.gameObject);
    }

    public virtual void SetFocus(Interactable newFocus)
    {
        focus = newFocus;
        Debug.Log(gameObject.name + " focusing on " + newFocus.GetComponent<Collider>().name);
        StartCoroutine(FollowTarget());
    }

    public virtual void Defocus()
    {
        if(focus != null)
        {
            focus = null;
            StopFollowingTarget();
            Debug.Log(gameObject.name + " Defocusing");
        }
    }

    public virtual void Move(Vector3 point)
    {
        agent.SetDestination(point);
        Debug.Log(gameObject.name + " Moving to " + point);
    }

    public virtual IEnumerator FollowTarget()
    {
        while(true)
        {
            if(focus != null)
            {
                agent.SetDestination(focus.transform.position);
            }
            yield return null;
        }
    }

    public virtual void StopFollowingTarget()
    {
        StopCoroutine(FollowTarget());
    }

    public virtual void Attack(Interactable target)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    #region Global Variables
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Interactable focus;
    public bool hasInteractedWFocus = false;

    [SerializeField] private UnitData unitData;
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
        // Check if this is already focused on another interactable
        if (newFocus != focus && focus != null)
        {
            focus.OnDefocused(transform);
        }
        // Set the interactable as the focus for this
        focus = newFocus;
        // Notify focus its is selected
        newFocus.OnFocused(transform);
        // Start following the focus
        StartCoroutine(FollowTarget());
        hasInteractedWFocus = false;
        Debug.Log(gameObject.name + " focusing on " + newFocus.GetComponent<Collider>().name);
    }

    public virtual void Defocus()
    {
        if(focus != null)
        {
            focus.OnDefocused(transform);
            focus = null;
            StopFollowingTarget();
            hasInteractedWFocus = false;
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
        Debug.Log(gameObject.name + " Following " + focus);

        while (true)
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
}

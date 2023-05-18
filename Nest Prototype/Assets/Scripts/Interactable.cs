using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // The min distance a unit needs to be to interact with this 
    [SerializeField] private float interactionRadius = 2.5f;
    [SerializeField] private bool isFocus = false;
    // The units that are currently focusing on this
    [SerializeField] private List<Transform> focusingUnits = new List<Transform>();
    // A list of bools tracking if each focusing unit has interacted with this yet

    private void Update()
    {
        // While this is the focus of a unit...
       if (isFocus)
        {
            // Loop through every unit focusing on this
            foreach (Transform unit in focusingUnits)
            {
                float distance = Vector3.Distance(unit.position, transform.position);

                // Checks if the unit is close enough to interact with this
                if (distance <= interactionRadius && !unit.GetComponent<Unit>().hasInteractedWFocus)
                {
                    // Unit interacts with this
                    Interact(unit.GetComponent<Unit>());
                }
            }
        }
    }
    
    // When a unit focuses on this
    public void OnFocused(Transform unitTransform)
    {
        isFocus = true;
        // Check if this unit is already focusing on this
        if (!focusingUnits.Contains(unitTransform))
        {
            // Add this unit to the list of units focusing on this
            focusingUnits.Add(unitTransform);  
        }
       
    }

    // When a unit defocuses on this
    public void OnDefocused(Transform unitTransform)
    {
        // Remove the unit defocusing on this from the list of units focusing on this
        focusingUnits.Remove(unitTransform);

        // Check if there are no remaining units focusing on this
        if (focusingUnits.Count < 1)
        {
            isFocus = false;
        }
    }

    public virtual void Interact(Unit unit)
    {
        unit.GetComponent<Unit>().hasInteractedWFocus = true;
        Debug.Log(unit.name + " is interacting with " + transform.name);
    }

    void OnDrawGizmosSelected () 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

}

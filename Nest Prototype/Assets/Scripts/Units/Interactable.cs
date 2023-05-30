using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float interactionRadius = 2.5f;              // The min distance a unit needs to be to interact with this
    [SerializeField] private bool  isFocus = false;                       
    public List<Transform>         focusingUnits = new List<Transform>(); // The units that are currently focusing on this
    public InteractableType        interactableType;
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
                if (distance <= interactionRadius && !unit.GetComponent<Unit>().isInteracting)
                {
                    // Unit interacts with this
                    unit.GetComponent<Unit>().isInteracting = true;
                    unit.GetComponent<Unit>().Interact();
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (Transform unit in focusingUnits.ToArray())
        {
            unit.GetComponent<Unit>().Defocus();
        }
        Destroy(gameObject);
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
     void OnDrawGizmosSelected () 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    } 

}

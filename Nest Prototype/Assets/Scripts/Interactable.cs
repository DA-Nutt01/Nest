using UnityEngine;

public class Interactable : MonoBehaviour
{
    // The min distance a unit needs to be to interact with this 
    public float interactionRadius = 2.5f;

    void OnDrawGizmosSelected () 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

}

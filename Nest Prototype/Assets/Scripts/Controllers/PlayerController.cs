using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Global Variables
    public LayerMask interactableLayer;
    public Interactable selectedObject;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // When Left click on the mouse is pressed...
        {
            // A ref to the object hit by the raycast;
            RaycastHit hit;  

            // Create a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 

            // If the ray hit an object on the interatable layer...
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                // Store the Interactable script component off the object that was just hit in a var ref
                Interactable interactable = hit.collider.GetComponent<Interactable>();

                // If the the object we just hit has an Interactable component...
                if(interactable != null)
                {
                    // Set the interactable as the focus
                    selectedObject = interactable;
                    Debug.Log("Selected " + interactable.gameObject.name);
                }
            } else
            {
                selectedObject = null;
                Debug.Log("Defocusing");
            }

            
        }
    }
}

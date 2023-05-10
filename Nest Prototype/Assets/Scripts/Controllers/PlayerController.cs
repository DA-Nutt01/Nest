using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Global Variables
    // A mask to distinguish interactable objects from non interactable ones
    public LayerMask interactableLayer;
    public LayerMask movementLayer;
    // The current interactable the player has selected 
    public Interactable focus;
    // The current control state for the controller to differentiate when the controls need to change in each state of gameplay
    public ControlState controlState;
    // Ref to the currentle selected unit if there is one
    private Unit selectedUnit;
    // Ref to the main camera
    Camera cam;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        controlState = ControlState.defocused;
    }

    // Update is called once per frame
    void Update()
    {

        switch(controlState)
        {
            case ControlState.defocused:
                if(Input.GetMouseButtonDown(0)) // On Left Mouse Click
                {
                    // A ref to the object hit by the raycast;
                    RaycastHit hit;  

                    // Create a ray from the camera to the mouse position
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition); 

                    // If the ray hit an object on the interatable layer...
                    if(Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
                    {
                        // Store the Interactable script component off the object that was just hit in a var ref
                        Interactable interactable = hit.collider.GetComponent<Interactable>();

                        // If the the object we just hit has an Interactable component...
                        if(interactable != null)
                        {
                            // Set the interactable as the focus
                            SetFocus(interactable);
                            controlState = ControlState.unitSelected;
                        }
                    }
                }
                break;
                
            case ControlState.unitSelected:
                if(Input.GetMouseButtonDown(0)) //On Left Mouse Click...
                    {
                    // A ref to the object hit by the raycast;
                    RaycastHit hit;  

                    // Create a ray from the camera to the mouse position
                    Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                    // If the ray hit anything...
                    if(Physics.Raycast(ray, out hit, 100f))
                    {
                        // Grab a ref to the interacable component of the hit if it has one
                        Interactable interactable = hit.collider.GetComponent<Interactable>();

                        // If the hit is in fact an interactable...
                        if(interactable != null)
                        {
                            // Move to that interactable
                            selectedUnit.Move(hit.point);
                            Debug.Log("Moving & Interacting");
                            // Interact with that interactable

                        } else {
                            //Move to that position if it is walkable
                            selectedUnit.Move(hit.point);
                            Debug.Log("Just Moving");
                        }
                    } 
                }
                
                break;
        }

        if(Input.GetMouseButtonDown(1)) // On Right Mouse Click
        {
            Defocus();
        }
    }

    private void SetFocus(Interactable interactable)
    {
        // Checks if the interactable is a Unit or not
        switch (interactable.GetComponent<Unit>())
        {
            case null: // Interactable is not a unit
                // Set current focus to this interactable
                focus = interactable;
                break;
            default: // Interactable is a unit
                // Set current focus to this interactable
                focus = interactable;
                // Grab ref to Unit component to access this unit's functions
                selectedUnit = interactable.GetComponent<Unit>();
                // Set unit as the camera's target to follow
                cam.GetComponent<CameraController>().StartFollow(focus);
                break;
        }
    }

    private void Defocus()
    {
        // We only need to defocus if an interactable is currently being focused on
        if(focus != null) 
        {
            focus = null;
            selectedUnit = null;
            controlState = ControlState.defocused;
            cam.GetComponent<CameraController>().StopFollow();
            Debug.Log("Defocusing");
        }
    }
}

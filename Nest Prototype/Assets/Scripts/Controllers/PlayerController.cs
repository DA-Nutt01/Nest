using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Global Variables
    // A mask to distinguish interactable objects from non interactable ones
    public LayerMask interactableLayer;
    public LayerMask movementLayer;
    // The current interactable the controller has selected 
    public Interactable focus;
    // The current control state for the controller to differentiate when the controls need to change in each state of gameplay
    public ControlState controlState;
    // Ref to the currentle selected unit if there is one
    private Unit selectedUnit;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
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
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    // If the ray hit anything on the movement layer, such as the ground...
                    if(Physics.Raycast(ray, out hit, Mathf.Infinity, movementLayer))
                    {
                        //Move the Unit to that location
                        selectedUnit.Move(hit.point);
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
                Debug.Log("Interactable is not a unit, Selected" + interactable.gameObject.name);
                // Set current focus to this interactable
                focus = interactable;
                break;
            default: // Interactable is a unit
                Debug.Log("Interactable is a unit, Selected " + interactable.gameObject.name);
                // Set current focus to this interactable
                focus = interactable;
                // Grab ref to Unit component to access this unit's functions
                selectedUnit = interactable.GetComponent<Unit>();
                break;
        }
    }

    private void Defocus()
    {
        // We only need to defocus if an interactable is currently being focused on
        if(focus != null) 
        {
            focus = null;
            controlState = ControlState.defocused;
            selectedUnit = null;
            Debug.Log("Defocusing");
        }
    }
}

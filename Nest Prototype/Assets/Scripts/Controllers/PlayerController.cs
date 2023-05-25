using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Global Variables
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask movementLayer;
    [SerializeField] private ControlState controlState = ControlState.Defocused;
    Camera cam;

    //Box Selector Variables
    [SerializeField] private RectTransform boxVisual;

    private Rect selectionBox;
    private Vector2 boxStartPosition;
    private Vector2 boxEndPosition;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        boxStartPosition = Vector2.zero;
        boxEndPosition = Vector2.zero;
        DrawVisual();
    }

    // Update is called once per frame
    void Update()
    {
        // When left mouse clicked
        if (Input.GetMouseButtonDown(0))
        {
            boxStartPosition = Input.mousePosition;
            selectionBox = new Rect();
        }

        // While left mouse dragged
        if (Input.GetMouseButton(0))
        {
            boxEndPosition = Input.mousePosition;
            DrawVisual();
            DrawSelection();
        }

        // When left mouse released
        if (Input.GetMouseButtonUp(0))
        {
            SelectUnitsInBox();
            boxStartPosition = Vector2.zero;
            boxEndPosition = Vector2.zero;
            DrawVisual();
        }

        // Unit Selection
        if(Input.GetMouseButtonDown(0)) // On Left Mouse Click
        {
            // A ref to the object hit by the raycast;
            RaycastHit hit;  

            // Create a ray from the camera to the mouse position
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); 

            // If the ray hit an object on the interatable layer...
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
            {
                // If the player is holding down shift...
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    // Add this unit to the selected units
                    UnitSelectionManager.Instance.ShiftClickSelect(hit.collider.gameObject);
                    controlState = ControlState.UnitsSelected;
                } else
                {
                    // Remove all over units from selection & add this one
                    UnitSelectionManager.Instance.ClickSelect(hit.collider.gameObject);
                    controlState = ControlState.UnitsSelected;
                }

            } else // If our ray hit nothing...
            {
                // If the player is not holding down shift...
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    // Deselect all units
                    UnitSelectionManager.Instance.DeselectAll();
                    controlState = ControlState.Defocused;
                }   
                        
            }
        }


        if (controlState == ControlState.UnitsSelected)
        {
                // Unit Movement
            if (Input.GetMouseButtonDown(1)) // On Right Click...
            {
                RaycastHit hit;  
                Ray ray = cam.ScreenPointToRay(Input.mousePosition); 

                // If the ray hit an interactable///
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
                {
                    // Cache the Interactable component of that interactable
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    // Loop through every selected unit
                    foreach (GameObject unitObject in UnitSelectionManager.Instance.selectedUnits)
                    {
                        Unit unit = unitObject.GetComponent<Unit>();
            
                        unit.SetFocus(interactable);
                        unit.FollowTarget();                        
                    }
                }
                else if(Physics.Raycast(ray, out hit, Mathf.Infinity, movementLayer))
                {
                    // Loop through every selected unit
                    foreach (GameObject unit in UnitSelectionManager.Instance.selectedUnits)
                    {
                        unit.GetComponent<Unit>().Move(hit.point);
                        unit.GetComponent<Unit>().Defocus();
                    }
                }

            }
        }
    }

    // Draws visual box graphic on screen while dragging
    void DrawVisual()
    {
        Vector2 boxStart = boxStartPosition;
        Vector2 boxEnd = boxEndPosition;

        Vector2 boxCenter = (boxStart + boxEnd) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));

        boxVisual.sizeDelta = boxSize;
    }

    void DrawSelection()
    {
        // X Calculations
        if (Input.mousePosition.x < boxStartPosition.x)
        {
            // Player is dragging box left; must invert x
            selectionBox.xMin = Input.mousePosition.x;
            selectionBox.xMax = boxStartPosition.x;
        } else
        {
            // Player is dragging box right
            selectionBox.xMin = boxStartPosition.x;
            selectionBox.xMax = Input.mousePosition.x;
        }

        // Y Calculations
        if (Input.mousePosition.y < boxStartPosition.y)
        {
            // Player is dragging box down
            selectionBox.yMin = Input.mousePosition.y;
            selectionBox.yMax = boxStartPosition.y;
        } else
        {
            // Player is dragging box up
            selectionBox.yMin = boxStartPosition.y;
            selectionBox.yMax = Input.mousePosition.y;
        }
    }

    public void SelectUnitsInBox()
    {
        // Loop through every unit in the scene
        foreach (GameObject unit in UnitSelectionManager.Instance.allUnits)
        {
            // If the unit is within bounds of the selection box...
            if (selectionBox.Contains(cam.WorldToScreenPoint(unit.transform.position)))
            {
                // Select that unit
                UnitSelectionManager.Instance.DragSelect(unit);
                controlState = ControlState.UnitsSelected;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Global Variables
    private Camera                        cam;
    [SerializeField] private LayerMask    selectionMask;
    [SerializeField] private LayerMask    enemyMask;
    [SerializeField] private LayerMask    movementMask;
    [SerializeField] private ControlState controlState = ControlState.Defocused;

    //Box Selector Variables
    [SerializeField] private RectTransform boxVisual;

    private Rect selectionBox;
    private Vector2 boxStartPosition;
    private Vector2 boxEndPosition;


    private static PlayerController _instance;
    public static PlayerController  Instance {get {return _instance;}}
    #endregion

    void Awake() 
    {
        // If an instance of this already exists and it isn't this one...
        if(_instance != null && _instance != this)
        {
            // Destroy this instance (There can only be one)
            Destroy(this.gameObject);
        } else {
            // Set the instance of this script to this instance
            _instance = this;
        }
    }
    
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
            if(Physics.Raycast(ray, out hit, Mathf.Infinity, selectionMask))
            {
                switch (hit.transform.gameObject.layer)
                {
                    case (6): // Alien Unit
                        // If the player is holding down shift...
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            // Add this unit to the selected units
                            UnitSelectionManager.Instance.ShiftClickSelect(hit.collider.gameObject);
                            
                        }
                        else
                        {
                            // Remove all over units from selection & add this one
                            UnitSelectionManager.Instance.ClickSelect(hit.collider.gameObject);
                            controlState = ControlState.UnitsSelected;
                        }

                        StructureSelectionManager.Instance.Deselect();
                        controlState = ControlState.UnitsSelected;
                        break;
                    case (7): //Alien Structure
                        UnitSelectionManager.Instance.DeselectAll();
                        // Select Structure
                        StructureSelectionManager.Instance.SelectStructure(hit.collider.gameObject);
                        controlState = ControlState.StructureSelected;
                        break;
                }
                

            } else // ray hit nothing
            {
                StructureSelectionManager.Instance.Deselect();
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

                // If the ray hit an interactable on the enemy mask (Unit/Strucutre)
                if(Physics.Raycast(ray, out hit, Mathf.Infinity, enemyMask))
                {
                    // Cache the Interactable component of that interactable
                    Interactable interactable = hit.collider.GetComponent<Interactable>();

                    // Loop through every selected unit
                    foreach (GameObject unitObject in UnitSelectionManager.selectedUnits)
                    {
                        // cache the Unit component on this unit
                        Unit unit = unitObject.GetComponent<Unit>();

                        // set the selected interactable as this unit's focus 
                        unit.SetFocus(interactable);

                        // Have this unit start to follow the selected interactable
                        unit.FollowTarget();                        
                    }
                }
                
                else if(Physics.Raycast(ray, out hit, Mathf.Infinity, movementMask)) // If the ray hits the ground and no other interactable
                {
                    // Cache the world position of the point the ray hit, ie the position to move selected units
                    Vector3 targetPosition = hit.point;

                    // Create a list of valid positions for the selected point
                    List<Vector3> targetPositionList = GetPositionAround(targetPosition, new float[] {4f, 8f, 12f}, new int[] {5, 10, 15});

                    // Create an indexer for selecting a sub position for each unit
                    int targetPositionListIndex = 0;

                    if (UnitSelectionManager.selectedUnits.Count == 1)
                    {
                        Unit currentUnit = UnitSelectionManager.selectedUnits[0].GetComponent<Unit>(); // Cache the unit component of this unit
                        currentUnit.Move(targetPosition, currentUnit.runSpeed);
                        currentUnit.Defocus();
                        return;
                    }

                    // Loop through every selected unit
                    foreach (GameObject unit in UnitSelectionManager.selectedUnits)
                    {
                        Unit currentUnit = unit.GetComponent<Unit>(); // Cache the unit component of this unit

                        // Move this unit to one of the sub postiions in the position list at their run speed
                        currentUnit.Move(targetPositionList[targetPositionListIndex], currentUnit.runSpeed); 

                        // Increment the list indexer for the next unit
                        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;

                        currentUnit.Defocus(); // Defocus this unit since it is no longer focused on an interactable
                    }
                }

            }
        }

        if (controlState == ControlState.StructureSelected)
        {
            //Check for input to begin spawn
            if (Input.GetMouseButtonDown(1))
            {
                StructureSelectionManager.Instance.selectedStructure.GetComponent<AlienHive>().SpawnUnits();
            }
        }
    }

    
    private List<Vector3> GetPositionAround(Vector3 startPosition, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        /// <summary>
        /// Generates a list of positions arranged in concentric rings around a specified point in 3D space.
        /// </summary>
        /// <param name="startPosition">The center point around which positions are arranged.</param>
        /// <param name="ringDistanceArray">An array of distances for each ring from the start position.</param>
        /// <param name="ringPositionCountArray">An array specifying the number of positions to generate in each ring.</param>
        /// <returns>A list of Vector3 positions arranged in concentric rings around the start position.</returns>

        // Create a new list to store all valid positions
        List<Vector3> positionList = new List<Vector3>();
        
        // Add the start position to the list
        positionList.Add(startPosition);

        // Loop through each ring distance and position count
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            // Generate positions around the start position for the current ring
            // and add them to the position list
            positionList.AddRange(GetPositionListAroundPoint(startPosition, ringDistanceArray[i], ringPositionCountArray[i]));
        }

        // Return the list of positions
        return positionList;
    }

    private List<Vector3> GetPositionListAroundPoint(Vector3 startPosition, float distance, int positionCount)
    {
        /// <summary>
        /// Generates a list of positions arranged evenly around a specified point in 3D space.
        /// </summary>
        /// <param name="startPosition">The center point around which positions are arranged.</param>
        /// <param name="distance">The distance from the start position to each generated position.</param>
        /// <param name="positionCount">The number of positions to generate around the start position.</param>
        /// <returns>A list of Vector3 positions evenly distributed around the start position.</returns>

        List<Vector3> positionList = new List<Vector3>(); // Create a new list to store all valid positions

        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360f / positionCount); // Calculate the angle for each position
            Vector3 dir = Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward; // Get the direction vector for this angle in 3D
            Vector3 position = startPosition + dir * distance; // Calculate the position based on direction and distance
            positionList.Add(position); // Add the calculated position to the list
        }

        return positionList; // Return the list of positions
    }


    private Vector3 ApplyRotationToVector(Vector3 vec, float angle)
    {
        /// <summary>
        /// Applies a 2D rotation to a vector around the z-axis by the specified angle.
        /// </summary>
        /// <param name="vec">The vector to rotate.</param>
        /// <param name="angle">The angle in degrees by which to rotate the vector around the z-axis.</param>
        /// <returns>The rotated vector.</returns>

        return Quaternion.Euler(0, 0, angle) * vec;
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
        foreach (GameObject unit in UnitSelectionManager.allUnits)
        {
            // If the unit is within bounds of the selection box and is an alien unit...
            if (selectionBox.Contains(cam.WorldToScreenPoint(unit.transform.position)) && unit.GetComponent<Unit>().unitType == UnitType.Alien)
            {
                // Select that unit
                UnitSelectionManager.Instance.DragSelect(unit);
                controlState = ControlState.UnitsSelected;
            }
        }
    }

    public ControlState GetControlState()
    {
        return controlState;
    }
}

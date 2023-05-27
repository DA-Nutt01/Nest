using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    
    public static List<GameObject>      allUnits = new List<GameObject>();      // List of all units in the scene
    public static List<GameObject>      selectedUnits = new List<GameObject>(); // List of all units in the scene currently selected
    private static UnitSelectionManager _instance;
    public static UnitSelectionManager  Instance {get {return _instance;}}

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

    public void ClickSelect(GameObject unitToAdd)
    {
        // Clear all selected units
        DeselectAll();
        // Add this unit to selection list
        selectedUnits.Add(unitToAdd);
        ToggleSelectionGFX(unitToAdd);
    }

    public void ShiftClickSelect(GameObject unitToAdd)
    {
        // If this unit is not already one of our selected units...
        if (!selectedUnits.Contains(unitToAdd))
        {
            // Add this unit to selection list
            selectedUnits.Add(unitToAdd);
            ToggleSelectionGFX(unitToAdd);
        } else // This unit already is currently selected
        {
            // Remove unit from selection list
            selectedUnits.Remove(unitToAdd);
        }
    }

    public void DragSelect(GameObject unitToAdd)
    {
        // If the unit is not already selected...
        if (!selectedUnits.Contains(unitToAdd))
        {
            // Add the unit to selection list
            selectedUnits.Add(unitToAdd);
            // Toggle unit selection GFX
            ToggleSelectionGFX(unitToAdd);
        }
    }

    public void DeselectAll()
    {
        // Clear the list of all units
        selectedUnits.Clear();
        // Toggle off all selection GFX
        foreach (GameObject unit in allUnits)
        {
            ToggleSelectionGFX(unit);
        }
    }

    public void DeselectUnit(GameObject unitToDeselect)
    {
        
    }

    public void ToggleSelectionGFX(GameObject unit)
    {
        // If this unit is currently selected...
        if(selectedUnits.Contains(unit))
        {
            // Toggle selection GFX on
            unit.transform.GetChild(0).gameObject.SetActive(true);
        } else // This unit is not currently selected
        {
            // Toggle selection GFX off
            unit.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

}

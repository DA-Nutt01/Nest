using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSelectionManager : MonoBehaviour
{
    [SerializeField, Tooltip("The currently selected friendly structure")] 
    public GameObject                       selectedStructure;
    private static StructureSelectionManager _instance;

    public static StructureSelectionManager Instance { get { return _instance; } }

    void Awake()
    {
        // If an instance of this already exists and it isn't this one...
        if (_instance != null && _instance != this)
        {
            // Destroy this instance (There can only be one)
            Destroy(this.gameObject);
        }
        else
        {
            // Set the instance of this script to this instance
            _instance = this;
        }
    }

    public void SelectStructure(GameObject strucutre)
    {
        // Clear all selected units
        UnitSelectionManager.Instance.DeselectAll();
        // Select this structure
        selectedStructure = strucutre;
    }

    public void Deselect()
    {
        selectedStructure = null;
    }
}

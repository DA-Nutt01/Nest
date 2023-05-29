using UnityEngine;

[RequireComponent(typeof(Interactable))]
public abstract class Structure : MonoBehaviour
{
    [Header("Structure Settings"), Space(10)]

    [SerializeField, Tooltip("The scriptable object this derives data for initialization")]
    protected BaseStructureData structureData;

    [SerializeField, Tooltip("The type of structure this is")]
    protected StructureType type;  
    
    [SerializeField, Tooltip("Name of this structure")]
    protected new string    name;

    [SerializeField, Tooltip("The prefab that represents this structure")]
    protected GameObject prefab;            

    [Space(20), Header("Structure Stats"), Space(10)]

    [SerializeField, Tooltip("The maximum hit points of this structure")]
    protected int maxHitPoints;

    [SerializeField, Tooltip("The current hit points of this structure")]
    protected int currentHitPoints;

    [SerializeField, Tooltip("The percentage of damage ignored from damage sources")]
    protected int defense;

    protected virtual void Awake()
    {
        InitializeData(structureData);
        InitializeChild();
    }

    public virtual void InitializeData(BaseStructureData data)
    {
        structureData = data;
        type = data.type;
        name = data.name;
        prefab = data.prefab;

        maxHitPoints = data.maxHitPoints;
        currentHitPoints = data.currentHitPoints;
        defense = data.defense;

        // Call child-specific initialization method
        InitializeChild();
    }
    protected abstract void InitializeChild();
}

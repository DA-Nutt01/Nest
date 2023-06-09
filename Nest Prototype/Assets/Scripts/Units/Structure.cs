using UnityEngine;

[RequireComponent(typeof(Health))]
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

    [SerializeField, Tooltip("Reference to Health component on this")]
    protected Health health;

    [SerializeField, Tooltip("The percentage of damage ignored from damage sources")]
    protected int defense;

    protected virtual void Awake()
    {
        InitializeData(structureData);
        InitializeChild();
    }

    protected virtual void InitializeData(BaseStructureData data)
    {
        structureData = data;

        type = data.type;
        name = data.name;
        prefab = data.prefab;

        health = GetComponent<Health>();
        health.maxHealth = data.maxHitPoints;
        health.currentHealth = data.currentHitPoints;
        defense = data.defense;

        // Call child-specific initialization method
        InitializeChild();
    }
    protected abstract void InitializeChild();
}

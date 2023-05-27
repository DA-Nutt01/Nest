using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Interactable))]
public class Structure : MonoBehaviour
{
    [SerializeField] private BaseStructureData structureData;
    [Header("Structure Settings")]
    [Space(10)]
    public StructureType structureType;
    public string        structureName;
    public GameObject    structurePrefab;
    [Space(20)]

    [Header("Structure Stats")]
    [Space(10)]
    public int baseHitPoints;
    public int baseArmor;


    // Start is called before the first frame update
    void Awake()
    {
        InitializeStructureData();
    }

    public virtual void InitializeStructureData()
    {
        structureType = structureData.structureType;
        baseHitPoints = structureData.baseHitPoints;
        baseArmor =     structureData.baseArmor;
    }
}

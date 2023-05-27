using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure Data", menuName = "Structure Data/Base Structure")]
public class BaseStructureData : ScriptableObject
{
    [Header("Structure Settings")]
    [Space(10)]
    public StructureType structureType;
    public string        structureName;
    public GameObject    StructurePrefab;
    [Space(20)]

    [Header("Structure Stats")]
    [Space(10)]
    public int baseHitPoints;
    public int baseArmor;
}

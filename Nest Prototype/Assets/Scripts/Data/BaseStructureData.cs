using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure Data", menuName = "Structure Data/Base Structure")]
public class BaseStructureData : ScriptableObject
{
    [Header("Structure Settings"), Space(10)]

    [Tooltip("The type of structure this is")]
    public StructureType type;

    [Tooltip("Name of this structure")]
    public new string    name;

    [Tooltip("The prefab that represents this structure")]
    public GameObject    prefab;

    [Space(20), Header("Structure Stats"), Space(10)]

    [Tooltip("The maximum hit points of this structure")]
    public int maxHitPoints;

    [Tooltip("The current hit points of this structure")]
    public int currentHitPoints;

    [Tooltip("The percentage of damage ignored from damage sources")]
    public int defense;
}

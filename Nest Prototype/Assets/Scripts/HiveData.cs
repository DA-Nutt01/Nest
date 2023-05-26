using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hive Data", menuName = "Structure Data/Hive Structure")]
public class HiveData : BaseStructureData
{ 
    [Header("Hive Settings"), Space(10)]
    [Tooltip("Prefab of unit this will spawn")]
    public GameObject unitToSpawn; 
    public int cost;
}

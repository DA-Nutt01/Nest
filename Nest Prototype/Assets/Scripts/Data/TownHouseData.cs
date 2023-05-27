using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Townhouse Data", menuName = "Structure Data/Townhouse Structure")]
public class TownHouseData : BaseStructureData
{ 
    [Header("Townhouse Settings"), Space(10)]
    [Tooltip("Prefab of unit this will spawn")]
    public GameObject unitToSpawn;
    [Tooltip("The radius in which units are spawned")]
    public float spawnRadius;

}

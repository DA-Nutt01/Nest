using UnityEngine;

[CreateAssetMenu(fileName = "New Hive Data", menuName = "Structure Data/Hive Structure")]
public class HiveData : BaseStructureData
{ 
    [Header("Hive Settings"), Space(10)]

    [Tooltip("Prefab of unit this will spawn")]
    public GameObject unitToSpawn;

    [Tooltip("The radius in which units are spawned")]
    public float spawnRadius;

    [Tooltip("Cost in Biomass to construct a hive")]
    public int cost;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : Structure
{
    [SerializeField,Header("Hive Settings"), Space(10)]
    private HiveData hiveData;
    [Tooltip("Prefab this hive will spawn")]
    public GameObject unitPrefab;
    [SerializeField ,Tooltip("Parent Game object units are nested under when spawned")]
    private GameObject parentObject;
    [Tooltip("The radius around the hive units are spawned")]
    public float spawnRadius;
    [Tooltip("Cost in Biomass to construct a hive")]
    public int cost;
    
     

    void Awake()
    {
        InitializeStructureData();
    }

    public override void InitializeStructureData()
    {
        base.InitializeStructureData();
        unitPrefab = hiveData.unitToSpawn;
        parentObject = GameObject.Find("Alien Units");
        spawnRadius = GetComponent<Interactable>().interactionRadius;
        cost = hiveData.cost;
    }
    public void SpawnUnits()
    {
        StartCoroutine(SpawnUnitWave());
    }

    public IEnumerator SpawnUnitWave()
    {
        int unitsToSpawn = unitPrefab.GetComponent<Unit>().squadSize; // Cache the sqaud size of the unit
        float spawnTime =  unitPrefab.GetComponent<Unit>().spawnTime; //Cache the spawn time of the unit
        Debug.Log($"Spawning {unitsToSpawn} units in {spawnTime} seconds");

        yield return new WaitForSeconds(spawnTime); // Let the unit's spawn time elapse before spawning units

        // Create a loop for each unit to spawn; for each unit to spawn
        for (int unitsSpawned = 0; unitsSpawned < unitsToSpawn; unitsSpawned++)
        {
            // Find a valid position within range of the hive to spawn a unit
            Vector3 spawnPosition = FindValidSpawnPosition();
            Debug.Log($"Spawn Position {unitsSpawned}: {spawnPosition}");
            // Spawn the unit at that position
            Instantiate(unitPrefab, spawnPosition, Quaternion.identity, parentObject.transform);
        }
    }

    private Vector3 FindValidSpawnPosition()
    {
        bool isValidPosition = false;
        Vector3 validSpawnPosition = Vector3.zero;
        
        while (isValidPosition == false)
        {
            Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;
            validSpawnPosition = transform.position + new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);

            // A list of any colliders that overlap with the potential spawn position
            Collider[] overlappingColliders = Physics.OverlapSphere(validSpawnPosition, 0.5f);

            // If ther are no overlapping colliders and position is within spawn radius...
            // && Vector3.Distance(validSpawnPosition, transform.position) > spawnRadius
            if (overlappingColliders.Length < 1)
            {
                isValidPosition = true;
                break;
            }    
                
        }

        return validSpawnPosition;
    }
}

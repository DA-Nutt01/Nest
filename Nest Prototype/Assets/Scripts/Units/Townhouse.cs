using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Townhouse : Structure
{
    [SerializeField, Tooltip("Reference to scriptable object for townhouse"), Header("Townhouse Settings"), Space(10)]
    private TownHouseData townhouseData;
    [Tooltip("Reference to scriptable object for human unit")]
    public BaseUnitData humanUnitData;
    [SerializeField ,Tooltip("Parent Game object units are nested under when spawned")]
    private GameObject parentObject;
    [Tooltip("The radius around the townhouse units are spawned")]
    public float spawnRadius;
    public bool isBusy = false;
    
     

    void Awake()
    {
        InitializeStructureData();
        InvokeRepeating("SpawnUnits", 3f, humanUnitData.spawnTime + 3f);
    }

    public override void InitializeStructureData()
    {
        base.InitializeStructureData();
        parentObject = GameObject.Find("Alien Units");
        spawnRadius = GetComponent<Interactable>().interactionRadius;
    }
    public void SpawnUnits()
    {
        // Check if this hive is already busy with another task
        // Check and spend amount of biomass to spawn units
        if (!isBusy)
            StartCoroutine(SpawnUnitWave());
        else
            Debug.Log("Hive is currently Busy");
    }

    public IEnumerator SpawnUnitWave()
    {
        isBusy = true;

        int unitsToSpawn = humanUnitData.squadSize; // Cache the sqaud size of the unit
        float spawnTime =  humanUnitData.spawnTime; //Cache the spawn time of the unit
        Debug.Log($"Spawning {unitsToSpawn} units in {spawnTime} seconds");

        yield return new WaitForSeconds(spawnTime); // Let the unit's spawn time elapse before spawning units

        // Create a loop for each unit to spawn; for each unit to spawn
        for (int unitsSpawned = 0; unitsSpawned < unitsToSpawn; unitsSpawned++)
        {
            // Find a valid position within range of the hive to spawn a unit
            Vector3 spawnPosition = FindValidSpawnPosition();
            // Spawn the unit at that position
            Instantiate(humanUnitData.unitPrefab, spawnPosition, Quaternion.identity, parentObject.transform);
        }

        isBusy = false;
        Debug.Log($"Units Spawned");
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
            if (overlappingColliders.Length < 1)
            {
                isValidPosition = true;
                break;
            }    
                
        }

        return validSpawnPosition;
    }
}

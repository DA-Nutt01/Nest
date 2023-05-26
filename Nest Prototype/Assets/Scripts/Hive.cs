using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : Structure
{
    [SerializeField,Header("Hive Settings"), Space(10)]
    private HiveData hiveData;
    [Tooltip("Prefab this hive will spawn")]
    public GameObject unitPrefab;
    [Tooltip("Cost in Biomass to construct a hive")]
    public int cost;
    [Tooltip("The radius around the hive units are spawned")]
    public float spawnRadius;

    void Awake()
    {
        InitializeStructureData();
    }

    public override void InitializeStructureData()
    {
        base.InitializeStructureData();
        cost = hiveData.cost;
        spawnRadius = GetComponent<Interactable>().interactionRadius;
    }
    public void SpawnUnits()
    {
        StartCoroutine(SpawnUnitWave());
    }

    public IEnumerator SpawnUnitWave()
    {
        
        int unitsToSpawn = unitPrefab.GetComponent<Unit>().squadSize;
        float startTime = Time.time;
        float endTime = startTime + unitPrefab.GetComponent<Unit>().spawnTime;
        Debug.Log($"Spawning {unitsToSpawn} units in {endTime - startTime} seconds");

        List<Vector3> spawnPositions = new List<Vector3>();

        while (Time.time < endTime)
        {
            // Check if a valid spawn position is available
            Vector3 spawnPosition = FindValidSpawnPosition();

            if (spawnPosition != Vector3.zero)
            {
                spawnPositions.Add(spawnPosition);
            }

            yield return null;
        }
        Debug.Log($"Spawn Positions: {spawnPositions.Count}");

        int unitsSpawned = 0;

        while (unitsSpawned < unitsToSpawn && spawnPositions.Count > 0)
        {
            // Randomly select a spawn position from the available positions
            int randomIndex = Random.Range(0, spawnPositions.Count);
            Vector3 spawnPosition = spawnPositions[randomIndex];

            // Spawn the unit at the selected position
            Instantiate(unitPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("Unit Spawned");
            // Remove the used spawn position from the list
            spawnPositions.RemoveAt(randomIndex);

            unitsSpawned++;
        } 
    }

    private Vector3 FindValidSpawnPosition()
    {
        // Calculate random position within the spawn radius
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;

        // Perform your validity check here (e.g., check if the position is obstructed, outside a boundary, etc.)
        // If the position is not valid, return Vector3.zero or perform another action based on your requirements

        return spawnPosition;
    }

}

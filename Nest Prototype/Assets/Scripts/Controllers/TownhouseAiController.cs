using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Townhouse))]
public class TownhouseAiController : MonoBehaviour
{
    [SerializeField] private Townhouse townHouse;
    void Awake()
    {
        townHouse = GetComponent<Townhouse>();
        //InvokeRepeating("SpawnUnits", 3f, townHouse.unitPrefab.GetComponent<Unit>().spawnTime + 3f);
        townHouse.SpawnUnits();
    }
}

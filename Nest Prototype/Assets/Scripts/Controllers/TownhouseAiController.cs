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
    }
}

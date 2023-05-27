using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "Unit Data/Base Unit")]
public class BaseUnitData : ScriptableObject
{
    [Header("Unit Settings")]
    [Space(10)]
    public UnitType       uniType;
    public string         unitName;
    public UnitAttackType attackType;
    public GameObject     unitPrefab;
    [Space(20)]

    [Header("Unit Stats")]
    [Space(10)]
    public int   baseAttackDamage;
    public float baseAttackRange;
    public float baseAttackRate; // # of attacks per second
    public int   baseHitPoints;
    public float baseMovementSpeed;
    public float baseDetectionRadius;
    public int   baseCost;
    [Tooltip("The number of this unit spawned when instantiated")]
    public int   squadSize;
    [Tooltip("Amount of time in sec for unit to be spawned")]
    public float spawnTime;
}

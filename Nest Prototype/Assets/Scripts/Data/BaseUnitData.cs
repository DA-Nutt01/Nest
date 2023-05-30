using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "Unit Data/Base Unit")]
public class BaseUnitData : ScriptableObject
{
    [Header("Unit Settings")]
    [Space(10)]
    public UnitType       unitType;
    public string         unitName;
    public UnitAttackType attackType;
    public GameObject     unitPrefab;
    [Space(20)]

    [Header("Unit Stats")]
    [Space(10)]
    public int   atackDamage;
    public float attackRange;
    [Tooltip("The number of attacks per second")]
    public float attackRate; 
    public int   maxHitPoints;
    public int   currentHitPoint;
    public float patrolSpeed;
    public float runSpeed;
    public float detectionRadius;
    public int   cost;
    [Tooltip("The number of this unit spawned when instantiated")]
    public int   squadSize;
    [Tooltip("Amount of time in sec for unit to be spawned")]
    public float spawnTime;
}

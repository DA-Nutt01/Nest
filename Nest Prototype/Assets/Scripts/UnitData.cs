using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "Unit Data")]
public class UnitData : ScriptableObject
{
    [Header("Unit Specs")]
    [Space(10)]
    public UnitType       uniType;
    public string         unitName;
    public GameObject     unitPrefab;
    [Space(20)]

    [Header("Unit Stats")]
    [Space(10)]
    public UnitAttackType attackType;
    public int            baseAttackDamage;
    public float          baseAttackRange;
    public int            baseHitPoints;
    public float          baseMovementSpeed;
    public float          baseDetectionRadius;
    public int            baseCost;
}

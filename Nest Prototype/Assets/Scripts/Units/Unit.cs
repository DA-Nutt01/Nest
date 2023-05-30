using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    #region Global Variables
    [Header("Unit Settings")]
    [Space(10)]
    [SerializeField] 
    public NavMeshAgent agent;
    public Interactable focus;
    public UnitType unitType;
    public UnitAttackType attackType;
    public bool isInteracting = false;
    public bool isAttacking = false;
    [Space(20)]

    [Header("Unit Stats")]
    [Space(10)]
    [SerializeField]
    private BaseUnitData unitData;
    public int           attackDamage;
    public float         attackRange;
    public float         attackRate;
    public int           maxHitPoints;
    public int           currentHitPoints;
    public float         patrolSpeed;
    public float         runSpeed;
    public float         detectionRadius;
    public int           cost;
    public int           squadSize;
    public float         spawnTime;
    
    
    #endregion

    private void Awake()
    {
        InitializeUnitData();
        agent = GetComponent<NavMeshAgent>();               // Cache a ref to this unit's nav mesh agent component
        UnitSelectionManager.allUnits.Add(this.gameObject); // Add this unit to list of all units in the game
        Defocus();                                          // Set focus to none
    }

    public void InitializeUnitData()
    {
        unitType =         unitData.unitType;
        attackType =       unitData.attackType;
        attackDamage =     unitData.atackDamage;
        attackRange =      unitData.attackRange;
        attackRate =       unitData.attackRate;
        maxHitPoints =     unitData.maxHitPoints;
        currentHitPoints = unitData.maxHitPoints;
        patrolSpeed =      unitData.patrolSpeed;
        runSpeed =         unitData.runSpeed;
        detectionRadius =  unitData.detectionRadius;
        cost =             unitData.cost;
        squadSize =        unitData.squadSize;
        spawnTime =        unitData.spawnTime;
    }

    public virtual void Die()
    {
        switch (unitType)
        {
            case (UnitType.Alien):
                UnitSelectionManager.selectedUnits.Remove(this.gameObject);
                break;
            case (UnitType.Human):
                break;
        }

        Defocus();
        Debug.Log($"{gameObject.name} has Died");
        UnitSelectionManager.allUnits.Remove(this.gameObject);
        Destroy(gameObject);
    }

    public virtual void SetFocus(Interactable newFocus)
    {
        // Check if this is already focused on another interactable
        if (newFocus != focus && focus != null)
        {
            focus.OnDefocused(transform);
        }
        // Set the interactable as the focus for this
        focus = newFocus;
        // Notify focus its is selected
        newFocus.OnFocused(transform);
        // Start following the focus
        StartCoroutine(FollowTarget());
        isInteracting = false;
    }

    public virtual void Defocus()
    {
        if(focus != null)
        {
            focus.OnDefocused(transform);
            focus = null;
            StopFollowingTarget();
            isInteracting = false;
        }
    }

    public virtual void Move(Vector3 point, float moveSpeed)
    {
        agent.speed = moveSpeed;
        agent.SetDestination(point);
    }

    public virtual IEnumerator FollowTarget()
    {
        agent.speed = runSpeed;

        while (true)
        {
            if(focus != null)
            {
                agent.SetDestination(focus.transform.position); // Follow target
            }
            yield return null;
        }
    }

    public virtual IEnumerator AttackTarget()
    { 
        while (focus)
        {
            Debug.Log($"{gameObject.name} dealt {attackDamage} damage to {focus.name}");
            focus.GetComponent<Unit>().ChangeHealth(-attackDamage);
            yield return new WaitForSeconds(1f / attackRate);
        }
    }

    public virtual void ChangeHealth(int amount)
    {
        currentHitPoints += amount;
        if (currentHitPoints <= 0) Die();
    }

    public virtual void StopFollowingTarget()
    {
        StopCoroutine(FollowTarget());
    }

    public virtual void Interact()
    {
        switch (focus.interactableType)
        {
            case (InteractableType.Unit):
                Debug.Log($"{gameObject.name} attacking {focus.name}");
                StartCoroutine(AttackTarget());
                break;
            case (InteractableType.Structure):
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
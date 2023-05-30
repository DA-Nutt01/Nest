using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Unit : MonoBehaviour
{
    #region Global Variables
    [Header("Unit Settings"), Space(10)]
    [Tooltip("Reference to NavMeshAgent component on this")]
    public NavMeshAgent agent;
    [Tooltip("Reference to Interactable component on this")]
    public Interactable focus;
    public UnitType unitType;
    public UnitAttackType attackType;
    [Space(10)]
    public bool isInteracting = false;
    public bool isAttacking = false;
    [Space(20)]

    [Header("Unit Stats")]
    [Space(10)]
    [SerializeField]
    private BaseUnitData unitData;
    public Health        health;
    public int           attackDamage;
    public float         attackRange;
    public float         attackRate;
    public float         patrolSpeed;
    public float         runSpeed;
    public float         detectionRadius;
    public int           cost;
    public int           squadSize;
    public float         spawnTime;
    
    
    #endregion

    private void Awake()
    {
        InitializeUnitData(unitData);                                
    }

    public void InitializeUnitData(BaseUnitData data)
    {
        agent = GetComponent<NavMeshAgent>();
        UnitSelectionManager.allUnits.Add(this.gameObject); // Add this unit to list of all units in the game
        Defocus();                                          // Set focus to none

        unitData = data;
        unitType =             data.unitType;
        health = GetComponent<Health>();
        health.maxHealth =     data.maxHealth;
        health.currentHealth = data.currentHealth;
        attackType =           data.attackType;
        attackDamage =         data.atackDamage;
        attackRange =          data.attackRange;
        attackRate =           data.attackRate;
        patrolSpeed =          data.patrolSpeed;
        runSpeed =             data.runSpeed;
        detectionRadius =      data.detectionRadius;
        cost =                 data.cost;
        squadSize =            data.squadSize;
        spawnTime =            data.spawnTime;
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

    public virtual IEnumerator AttackTarget(Interactable target)
    { 
        while (target && Vector3.Distance(transform.position, focus.transform.position) < focus.interactionRadius)
        {
            Debug.Log($"{gameObject.name} dealt {attackDamage} damage to {focus.name}");
            target.GetComponent<Health>().TakeDamage(attackDamage);
            yield return new WaitForSeconds(1f / attackRate);
        }
    }

    public virtual void StopFollowingTarget()
    {
        StopAllCoroutines();
    }

    public virtual void Interact()
    {
        switch (focus.interactableType)
        {
            case (InteractableType.Unit):
                Debug.Log($"{gameObject.name} attacking {focus.name}");
                StartCoroutine(AttackTarget(focus));
                break;
            case (InteractableType.Structure):
                StartCoroutine(AttackTarget(focus));
                break;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

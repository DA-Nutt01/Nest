 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(Interactable))]
public abstract class Unit : MonoBehaviour
{
    #region Global Variables
    [Header("Unit Settings"), Space(10)]

    [SerializeField, Tooltip("NavMeshAgent component on thi  s")]
    public NavMeshAgent agent;

    [SerializeField, Tooltip("Interactable this unit is currently focusing on; Can only have one focus at a time")]
    public Interactable focus;

    [SerializeField, Tooltip("Type of unit this is")]
    public UnitType unitType;

    [SerializeField, Tooltip("Primary way this unit attacks"), Space(10)]
    protected UnitAttackType attackType;

    [SerializeField, Tooltip("If this is currently interacting or not")]
    public bool isInteracting = false;

    [SerializeField, Tooltip("If this is currently attacking or not")]
    public bool isAttacking = false;
    
    [Space(20),Header("Unit Stats"), Space(10)]
    
    [SerializeField, Tooltip("Scriptable Object this derives data from")]
    protected BaseUnitData unitData;

    [SerializeField, Tooltip("Parent Game object units are nested under when spawned")]
    protected GameObject parentObject;

    [SerializeField, Tooltip("Health component attached to this")]
    protected Health health;

    [SerializeField, Tooltip("Amount of damage this deals each attack")]
    public int attackDamage;

    [SerializeField, Tooltip("Distance from its target this can start attacking from")]
    public float attackRange;

    [SerializeField, Tooltip("Number of times this attacks per second")]
    public float attackRate;

    [SerializeField, Tooltip("Movement speed of this while patroling or idle movement")]
    public float patrolSpeed;

    [SerializeField, Tooltip("Movement speed of this outside of patroling")]
    public float runSpeed;

    [SerializeField, Tooltip("The distance from this that other units are detected from")]
    public float detectionRadius;

    [SerializeField, Tooltip("Number of this unit spawned when spawned")]
    public int squadSize;

    [SerializeField, Tooltip("Time in seconds it takes to spawn this unit")]
    public float spawnTime;

    [SerializeField, Tooltip("Cost in resources to spawn a sqaud of this unit")]
    public int cost;

    #endregion

    protected virtual void Awake()
    {
        InitializeUnitData(unitData);                                
    }

    protected virtual void InitializeUnitData(BaseUnitData data)
    {
        unitData = data;

        agent = GetComponent<NavMeshAgent>();
        UnitSelectionManager.allUnits.Add(this.gameObject); 
           
        unitType =             data.unitType;
        health =               GetComponent<Health>();
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

        Defocus();
        InitializeChild(); // Call child-specific initialization method
    }

    protected abstract void InitializeChild();

    public abstract void Attack();

    protected virtual void OnDestroy()
    {
        Defocus();
        Debug.Log($"{gameObject.name} has Died");
        UnitSelectionManager.allUnits.Remove(this.gameObject);

        switch (unitType)
        {
            case (UnitType.Alien):
                UnitSelectionManager.selectedUnits.Remove(this.gameObject);
                UnitSelectionManager.alienUnits.Remove(this.gameObject);
                break;
            case (UnitType.Human):
                UnitSelectionManager.humanUnits.Remove(this.gameObject);
                break;
        }

        Destroy(gameObject);
    }

    public virtual void SetFocus(Interactable newFocus)
    {   
        if (newFocus != focus && focus != null) // Check if this is already focused on another interactable
        {
            focus.OnDefocused(transform);
        }
        
        focus = newFocus;               // Set the interactable as the focus for this
        newFocus.OnFocused(transform);  // Notify focus its is selected
        StartCoroutine(FollowTarget()); // Start following the focus
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
            isAttacking = false;
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

    protected virtual IEnumerator AttackTarget(Interactable target)
    {
        while (target != null && Vector3.Distance(transform.position, focus.transform.position) < focus.interactionRadius)
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
                Attack();
                break;
            case (InteractableType.Structure):
                Attack();
                break;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

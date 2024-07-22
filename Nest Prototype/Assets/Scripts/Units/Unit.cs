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

    [SerializeField, Tooltip("NavMeshAgent component on this")] 
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
    
    [Space(20), Header("Unit Stats"), Space(10)] 
    
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

    /// <summary>
    /// Initializes the unit data when the unit awakens.
    /// </summary>
    protected virtual void Awake()
    {
        InitializeUnitData(unitData); 
    }

    /// <summary>
    /// Initializes the unit's data with provided BaseUnitData.
    /// </summary>
    protected virtual void InitializeUnitData(BaseUnitData data)
    {
        unitData = data;

        agent = GetComponent<NavMeshAgent>();
        UnitSelectionManager.allUnits.Add(this.gameObject); 
           
        unitType = data.unitType;
        health = GetComponent<Health>(); 
        health.maxHealth = data.maxHealth; 
        health.currentHealth = data.currentHealth; 
        attackType = data.attackType; 
        attackDamage = data.attackDamage;
        attackRange = data.attackRange; 
        attackRate = data.attackRate; 
        patrolSpeed = data.patrolSpeed; 
        runSpeed = data.runSpeed;
        detectionRadius = data.detectionRadius; 
        cost = data.cost; 
        squadSize = data.squadSize; 
        spawnTime = data.spawnTime; 

        Defocus(); 
        InitializeChild(); 
    }

    /// <summary>
    /// Abstract method to be implemented by child classes for specific initialization.
    /// </summary>
    protected abstract void InitializeChild();

    /// <summary>
    /// Abstract method for attacking, to be implemented by child classes.
    /// </summary>
    public abstract void Attack();

    /// <summary>
    /// Handles the unit's destruction, including defocusing and removing from selection managers.
    /// </summary>
    protected virtual void OnDestroy()
    {
        Defocus(); 
        Debug.Log($"{gameObject.name} has Died"); 
        UnitSelectionManager.allUnits.Remove(this.gameObject); // Remove this unit from the allUnits list

        switch (unitType) // Check the unit type
        {
            case (UnitType.Alien):
                UnitSelectionManager.selectedUnits.Remove(this.gameObject); // Remove from selected units
                UnitSelectionManager.alienUnits.Remove(this.gameObject); // Remove from alien units
                break;
            case (UnitType.Human):
                UnitSelectionManager.humanUnits.Remove(this.gameObject); // Remove from human units
                break;
        }

        Destroy(gameObject); 
    }

    /// <summary>
    /// Sets a new focus for the unit and starts following it.
    /// </summary>
    public virtual void SetFocus(Interactable newFocus)
    {   
        if (newFocus != focus && focus != null) // Check if this is already focused on another interactable
        {
            focus.OnDefocused(transform); // Call OnDefocused on the current focus
        }
        
        focus = newFocus;               // Set the interactable as the focus for this
        newFocus.OnFocused(transform);  // Notify focus it is selected
        StartCoroutine(FollowTarget()); // Start following the focus
        isInteracting = false;          // Set isInteracting to false
    }

    /// <summary>
    /// Clears the current focus of the unit.
    /// </summary>
    public virtual void Defocus()
    {
        if(focus != null) // Check if there is a current focus
        {
            focus.OnDefocused(transform); 
            focus = null; 
            StopFollowingTarget(); 
            isInteracting = false;
            isAttacking = false; 
        }
    }

    /// <summary>
    /// Moves the unit to a specified point with a given speed.
    /// </summary>
    public virtual void Move(Vector3 point, float moveSpeed)
    {
        agent.speed = moveSpeed;    
        agent.SetDestination(point); 
    }

    /// <summary>
    /// Coroutine for following the target focus.
    /// </summary>
    public virtual IEnumerator FollowTarget()
    {
        agent.speed = runSpeed; // Set the agent's speed to run speed

        while (true)
        {
            if(focus != null) // Check if there is a focus
            {
                agent.SetDestination(focus.transform.position); 
            }
            yield return null; // Wait for the next frame
        }
    }

    /// <summary>
    /// Coroutine for attacking the focused target.
    /// </summary>
    protected virtual IEnumerator AttackTarget(Interactable target)
    {
        while (target != null && Vector3.Distance(transform.position, focus.transform.position) < focus.interactionRadius) // Check if target is within attack range
        {
            Debug.Log($"{gameObject.name} dealt {attackDamage} damage to {focus.name}"); 
            target.GetComponent<Health>().TakeDamage(attackDamage); // Deal damage to the target
            yield return new WaitForSeconds(1f / attackRate); // Wait for the next attack based on attack rate
        }
    }

    /// <summary>
    /// Stops the unit from following the current target.
    /// </summary>
    public virtual void StopFollowingTarget()
    {
        StopAllCoroutines(); 
    }

    /// <summary>
    /// Handles the interaction logic when the unit reaches its focus.
    /// </summary>
    public virtual void Interact()
    {
        switch (focus.interactableType) // Check the type of the interactable
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

    /// <summary>
    /// Draws a wire sphere in the editor to visualize the unit's detection radius.
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // Set Gizmo color to red
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // Draw wire sphere at the unit's position with the detection radius
    }
}
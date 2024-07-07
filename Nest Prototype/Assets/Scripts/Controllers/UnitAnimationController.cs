using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
public class UnitAnimationController : MonoBehaviour
{
    #region Global Variables
    [Header("Settings"), Space(10)]

    private Animator animator;
    private NavMeshAgent agent;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (agent.hasPath && agent.velocity.sqrMagnitude > 0.1f)
        {
            animator.SetBool("isRunning", true);
        } else 
        {  
            animator.SetBool("isRunning", false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class UnitAnimationController : MonoBehaviour
{
    #region Global Variables
    [Header("Settings"), Space(10)]

    private Animator animator;
    private Rigidbody rb;
    #endregion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            // Unit is moving
            animator.SetBool("isRunning", true);
        }
        else
        {
            // Unit is not moving
            animator.SetBool("isRunning", false);
        }
    }
}

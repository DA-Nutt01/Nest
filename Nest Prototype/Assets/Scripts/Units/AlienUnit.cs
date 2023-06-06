using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienUnit : Unit
{
    protected override void InitializeChild()
    {
        parentObject = GameObject.Find("Alien Units");
    }

    public override void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        StartCoroutine(AttackTarget(focus));
    }
}

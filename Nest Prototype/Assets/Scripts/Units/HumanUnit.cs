using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanUnit : Unit
{
    protected override void InitializeChild()
    {
        parentObject = GameObject.Find("Human Units");
    }

    public override void Attack()
    {
        isAttacking = true;
        StartCoroutine(AttackTarget(focus));
    }
}

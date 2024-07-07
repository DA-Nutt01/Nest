using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienUnit : Unit
{

    [SerializeField, Tooltip("A reference to this unit's selected graphic when being selected by the controller")]
    public GameObject selectionGFX;
    protected override void InitializeChild()
    {
        parentObject = GameObject.Find("Alien Units");
        UnitSelectionManager.alienUnits.Add(this.gameObject);
    }

    public override void Attack()
    {
        if (isAttacking) return;
        isAttacking = true;
        StartCoroutine(AttackTarget(focus));
    }

    public void ToggleSelectionGFX()
    {
        // If this unit is currently selected...
        if(UnitSelectionManager.selectedUnits.Contains(this.gameObject))
        {
            // Toggle selection GFX on
            selectionGFX.SetActive(true);
        } else // This unit is otherwise not currently selected
        {
            // Toggle selection GFX on
            selectionGFX.SetActive(false);
        }
    }
}

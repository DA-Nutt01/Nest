using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
[RequireComponent(typeof(Interactable))]
public class HumanAIController : MonoBehaviour
{
    //[SerializeField] private HumanAIControlState controlState;
    [SerializeField] private Unit                unit;
    [SerializeField] private Interactable        interactable;
    [SerializeField] private LayerMask           playerUnitLayer;

    // Start is called before the first frame update
    void Awake()
    {
        unit = GetComponent<Unit>();
        interactable = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly check if any player unit enters this unit's detection radius
        Collider[] detectedPlayerUnits = Physics.OverlapSphere(transform.position, unit.detectionRadius, playerUnitLayer);

        if (detectedPlayerUnits.Length >= 1 && unit.focus == null)
        {
            unit.SetFocus(detectedPlayerUnits[0].GetComponent<Interactable>());
        }

    }
}

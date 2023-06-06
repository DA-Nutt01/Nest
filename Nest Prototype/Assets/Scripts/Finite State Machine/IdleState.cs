using System.Collections;
using UnityEngine;

public class IdleState : BaseState
{
    public override void Awake()
    {

    }

    // Initialize relevant data and start state behavior
    public override void EnterState()
    {
        Debug.Log("IdleState: EnterState");
        StartCoroutine(UpdateState());
    }

    // Clean up any values, remove from active state list & destroy this
    public override void ExitState()
    {
        Debug.Log("IdleState: ExitState");
    }

    public override IEnumerator UpdateState()
    {
        Debug.Log("IdleState: UpdateState");

        // Perform idle behavior
        while (true)
        {
            // Perform idle actions or wait for specific conditions
            // Example: Waiting for a specific duration
            yield return new WaitForSeconds(2f);

            // Example: Waiting until a condition is true
            //while (!SomeCondition)
            //    yield return null;

            // Perform other actions or transitions based on conditions
        }
    }
}

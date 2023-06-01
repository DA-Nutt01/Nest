using BahaviorTree;

public class BehaviorTreeUnit : Tree
{
    public UnityEngine.Transform[] waypoints;
    public static float patrolSpeed = 2f;

    protected override Node SetupTree()
    {
        
        Node root = gameObject.AddComponent<TaskPatrol>() as TaskPatrol;
        GetComponent<TaskPatrol>().Initialize(transform, waypoints);


        return root;
    }
}

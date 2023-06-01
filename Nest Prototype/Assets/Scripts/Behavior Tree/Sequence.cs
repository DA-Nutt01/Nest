using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BahaviorTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override NodeState EvaluateState()
        {
            bool anyChildIsRunning = false;

            foreach (Node node in childNodes)
            {
                switch (node.EvaluateState())
                {
                    case NodeState.Failure:
                        nodeState = NodeState.Failure;
                        return nodeState;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        nodeState = NodeState.Success;
                        return nodeState;
                }
            }
            nodeState = anyChildIsRunning ? NodeState.Running : NodeState.Success;
            return nodeState;
        }
    }

}

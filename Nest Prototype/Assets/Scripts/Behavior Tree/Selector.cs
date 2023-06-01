using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BahaviorTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override NodeState EvaluateState()
        {
            foreach (Node node in childNodes)
            {
                switch (node.EvaluateState())
                {
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        nodeState = NodeState.Success;
                        return nodeState;
                    default:
                        continue;
                }
            }
            nodeState = NodeState.Failure;
            return nodeState;
        }
    }

}

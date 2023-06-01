using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BahaviorTree
{
    public enum NodeState
    {
        Running,
        Success,
        Failure
    }
    public class Node : MonoBehaviour
    {
        protected NodeState nodeState;

        public Node parent;                                    // The node this node is nested under
        protected List<Node> childNodes = new List<Node>(); // List of nodes nested under this node

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>(); // The _underscore is a naming convention that denotes this method or field is meant to be private and only accessed in this scope

        public Node()
        {
            /// <summary>
            ///  Base construcor with no parameters
            ///  Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>

            parent = null;
        }
        public Node(List<Node> childNodes)
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class with the specified list of child nodes.
            /// </summary>
            /// <param name="childNodes">The list of child nodes to add to the current node.</param>

            foreach (Node child in childNodes)
                _AddChildNode(child);
        }

        private void _AddChildNode(Node node)
        {
            /// <summary>
            /// Adds the specified <see cref="Node"/> as a child node to the current node.
            /// </summary>
            /// <param name="node">The <see cref="Node"/> to add as a child node.</param>

            node.parent = this;
            childNodes.Add(node);
        }
        public virtual NodeState EvaluateState() => NodeState.Failure;

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;

            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;
                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BahaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        private void Update()
        {
            if (_root != null)
                _root.EvaluateState();
        }
        protected abstract Node SetupTree();
    }
}


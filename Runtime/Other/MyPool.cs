using System;
using System.Collections.Generic;
using UnityEngine;





namespace Karianakis.Utilities
{
    public class MyPool<T>
    //where T : class
    {
        HashSet<T> Active { get; set; } = new();
        Stack<T> Inactive { get; set; } = new();
        public Func<T> Instantiate { private get; set; }
        public Action<T> Initialize { private get; set; }
        public Action<T> Deactivate { private get; set; }

        public T Get()
        {
            T node;
            int size = Inactive.Count;
            if (size > 0)
            {
                node = Inactive.Pop();
            }
            else
            {
                node = Instantiate();
            }
            Initialize(node);
            Active.Add(node);
            return node;
        }

        public void Remove(T node)
        {
            if (!Active.Contains(node)) { Debug.LogError("Attempted to remove a node that is not active."); return; }

            Active.Remove(node);
            Deactivate(node);
            Inactive.Push(node);
        }
    }
}



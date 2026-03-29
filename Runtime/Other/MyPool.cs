using System;
using System.Collections.Generic;
using System.Linq;
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
            if (Active.Contains(node) == false)
            {
                Type type = typeof(T);

                bool hasName = false;
                string monoName = "";

                if (typeof(MonoBehaviour).IsAssignableFrom(type))
                {
                    hasName = true;
                    monoName = (node as MonoBehaviour).name;

                }

                if (hasName == false)
                {
                    Debug.LogError($"unity-utilities : Attempted to remove a node that is not active. Type: {type}, NoName");
                }
                else
                {
                    Debug.LogError($"unity-utilities : Attempted to remove a node that is not active. Type: {type}, Name: {monoName}");
                }
                return;
            }

            Active.Remove(node);
            Deactivate(node);
            Inactive.Push(node);
        }
        public void RemoveAllActiveItems()
        {
            var array = Active.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                Remove(array[i]);
            }
        }
    
    }
}



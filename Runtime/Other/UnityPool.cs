using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Karianakis.Utilities
{
    public class UnityPool<T>
    //where T : class
    {
        public Transform _parent { private get; set; }
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
                if (_parent != null)
                {
                    if (node is MonoBehaviour mono)
                    {
                        mono.transform.SetParent(_parent);
                    }
                }
            }
            Initialize(node);
            Active.Add(node);
            return node;
        }

        public void Remove(T node)
        {
            MonoBehaviour monoNode = node as MonoBehaviour;

            if (Active.Contains(node))
            {
                if (_parent != null)
                {
                    if (node is MonoBehaviour monoAgain)
                    {
                        monoAgain.transform.SetParent(_parent);
                    }
                }

                Active.Remove(node);
                Deactivate(node);
                Inactive.Push(node);

            }
            else //! ERROR CASE
            {
                Type type = typeof(T);

                if (monoNode == null)
                {
                    Debug.LogError($"unity-utilities : Attempted to remove a node that is not active. Type: {type}, NoName");
                }
                else
                {
                    Debug.LogError($"unity-utilities : Attempted to remove a node that is not active. Type: {type}, Name: {monoNode.name}");
                }
                return;
            }

        }
        public void RemoveAllActiveItems()
        {
            var array = Active.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                Remove(array[i]);
            }
        }
        public T[] GetAllActiveItems()
        {
            var array = Active.ToArray();
            return array;
        }

    }
}



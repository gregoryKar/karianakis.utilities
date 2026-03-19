
using System.Collections.Generic;
using UnityEngine;

namespace Karianakis.Utilities
{
    public class InvoManager : MonoBehaviour
    {

        private static InvoManager instForbidden;
        private static InvoManager inst
        {
            get
            {
#if UNITY_EDITOR
                if (Application.isEditor && Application.isPlaying == false)
                {
                    Debug.LogError("DONT CALL INVO MANAGER ONLY IN EDIT MODE");
                    return null;
                }
#endif
                if (instForbidden == null)
                {
                    var gameObject = new GameObject("invoManager");
                    instForbidden = gameObject.AddComponent<InvoManager>();
                }
                return instForbidden;
            }
        }

        private List<InvoBase> _invokes = new();


        void Update()
        {
            while (_invokes.Count > 0)
            {
                var invo = _invokes[0];
                if (MyTime.mommentPassed(invo.GetEnd) is false) break;

                RemoveFirst();

                if (invo.GetKillMe is false)
                {
                    invo.InvokeMe(invo);
                    invo.Process();

                    if (invo.GetKillMe is false)
                    {
                        AddItemSorted(invo);
                    }
                }
            }

#if UNITY_EDITOR && KARIANAKIS
            
            if(_displayedImTestingLog == false)
            {
                _displayedImTestingLog = true;
                Debug.LogWarning($"init infinite invokes heap test");
            }

            _editHeapTestTimer += Time.deltaTime;
            if(_editHeapTestTimer >= _editHeapTestInterval)
            {
                _editHeapTestTimer = 0f;
                TestHeapValidity();
            }
#endif

        }

        
#if UNITY_EDITOR && KARIANAKIS
bool _displayedImTestingLog;
        float _editHeapTestTimer;
        const float _editHeapTestInterval = .5f;
#endif


        //? BASE FUNCTIONALITY
        // they call it push 
        void AddItemSorted(InvoBase thisOne)
        {
            _invokes.Add(thisOne);
            HeapifyUp(_invokes.Count - 1);
        }

        void RemoveItem(InvoBase thisOne)
        {
            int index = _invokes.IndexOf(thisOne);
            if (index == -1) return;

            if (index == _invokes.Count - 1)
            {
                _invokes.RemoveAt(index);
                return;
            }

            SwapPositions(index, _invokes.Count - 1);
            _invokes.RemoveAt(_invokes.Count - 1);

            // Only call one heapify direction as needed
            if (index > 0 && _invokes[index]
                .CompareTo(_invokes[(index - 1) / 2]) < 0)
            {
                HeapifyUp(index);
            }
            else
            {
                HeapifyDown(index);
            }
        }

        // they call it pop why do they also return the popped item ?
        void RemoveFirst()
        {
            if (_invokes.Count == 1)
            {
                _invokes.RemoveAt(0);
                return;
            }

            SwapPositions(0, _invokes.Count - 1);
            _invokes.RemoveAt(_invokes.Count - 1);
            HeapifyDown(0);

        }

        void HeapifyUp(int index)
        {
            while (index > 0)
            {
                int parentIndex = (index - 1) / 2;
                if (_invokes[index].CompareTo(_invokes[parentIndex]) >= 0)
                    break;

                SwapPositions(index, parentIndex);
                index = parentIndex;
            }
        }

        void HeapifyDown(int index)
        {
            int count = _invokes.Count;

            while (true)
            {
                int left = index * 2 + 1;
                int right = index * 2 + 2;
                int smallest = index;

                if (left < count && _invokes[left].CompareTo(_invokes[smallest]) < 0)
                    smallest = left;

                if (right < count && _invokes[right].CompareTo(_invokes[smallest]) < 0)
                    smallest = right;

                if (smallest == index) break;

                SwapPositions(index, smallest);
                index = smallest;
            }
        }

        void SwapPositions(int i, int j)
        {
            var temp = _invokes[i];
            _invokes[i] = _invokes[j];
            _invokes[j] = temp;
        }



        void TestHeapValidity()
        {

            for (int i = 0; i < _invokes.Count; i++)
            {
                int left = i * 2 + 1;
                int right = i * 2 + 2;

                if (left < _invokes.Count && _invokes[left].CompareTo(_invokes[i]) < 0)
                {
                    Debug.LogError($"Heap property violated at index {i} with left child {left}");
                }

                if (right < _invokes.Count && _invokes[right].CompareTo(_invokes[i]) < 0)
                {
                    Debug.LogError($"Heap property violated at index {i} with right child {right}");
                }
            }
        }

        //? FUNCTIONS FOR EXPOSED METHODS
        void KillAllLocal(MyId id)
        {
            if (id == null)
            {
                Debug.LogError(" KILL NULL ID ");
                return;
            }
            foreach (var item in inst._invokes)
            {
                if (item.GetId == null) continue;

                if (item.GetId.Equals(id))
                {
                    item.KillMe();
                }
            }
        }

        bool LookIfIdExists(MyId id)
        {
            foreach (var item in inst._invokes)
            {
                if (item.GetId == null) continue;

                if (item.GetId.Equals(id))
                {
                    return true;
                }
            }
            return false;
        }




        //? EXPOSED
        internal static void Add(InvoBase thisOne)
            => inst.AddItemSorted(thisOne);
        public static void KillAll(MyId id)
            => inst.KillAllLocal(id);
        public static bool Exists(MyId id)
            => inst.LookIfIdExists(id);

    }
}

using System;
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


        //? MASTERUPDATE ------------------------ ------------------------- ----
        void Update()//?
        {

            if (_invokes.Count == 0) return;

            List<InvoBase> _toEvoke = new();

            while (_invokes.Count > 0)
            {
                var invo = _invokes[0];
                if (MyTime.mommentPassed(invo.GetEnd) == false)
                {
                    break;
                }
                _toEvoke.Add(invo);
                RemoveFirst();
            }

            for (int i = 0; i < _toEvoke.Count; i++)
            {
                var invo = _toEvoke[i];

                if (invo.GetScheduledToDie)
                {
                    invo.MarkAsDead();
                    invo.TriggerDeathAction();

                }
                else if (invo.GetScheduledToEnd)
                {
                    invo.MarkAsDead();
                    invo.TriggerEndAction();
                }
                else if (invo.GetScheduledToPause)
                {
                    invo.ResetScheduledToPauseResume();
                    invo.SetPaused(true);
                    invo.SetEndTime(InvoBase._defaultPausedTime);
                    AddItemSorted(invo);
                }
                else if (invo.GetScheduledToResume)
                {
                    invo.ResetScheduledToPauseResume();
                    invo.SetPaused(false);
                    invo.SetEndFromSavedEndFromNow();
                    AddItemSorted(invo);
                }
                else //? MAIN CASE
                {
                    invo.InvokeMeBeforeProcessing(invo);
                    invo.ProcessAfterInvocation();

                    if (invo.GetCompleted == false)
                    {
                        AddItemSorted(invo);
                    }

                }
            }


#if UNITY_EDITOR && KARIANAKIS

            if (_displayedImTestingLog == false)
            {
                _displayedImTestingLog = true;
                Debug.LogWarning($"init infinite invokes heap test");
            }

            _editHeapTestTimer += Time.deltaTime;
            if (_editHeapTestTimer >= _editHeapTestInterval)
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
        void RemoveItemAndSort(InvoBase thisOne, out bool found)
        {
            int index = _invokes.IndexOf(thisOne);
            found = index != -1;

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
        void ReorderItemLocal(InvoBase thisOne)
        {
            int index = _invokes.IndexOf(thisOne);
            if (index == -1) return;
            // Try both directions, as value may have increased or decreased
            if (index > 0 && _invokes[index].CompareTo(_invokes[(index - 1) / 2]) < 0)
            {
                HeapifyUp(index);
            }
            else
            {
                HeapifyDown(index);
            }
        }




        //? DEBUG - EDIT
        void PrintNearIndex(int index, int distanceMax)
        {
            int start = Mathf.Max(0, index - distanceMax);
            int end = Mathf.Min(_invokes.Count - 1, index + distanceMax);

            for (int i = start; i <= end; i++)
            {
                if (i == index)
                {
                    Debug.LogError("INDEX START POINT");
                }

                Debug.Log($"Index {i}: {_invokes[i].GetEnd}");
            }
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
                    PrintNearIndex(i, 10);
                }

                if (right < _invokes.Count && _invokes[right].CompareTo(_invokes[i]) < 0)
                {
                    Debug.LogError($"Heap property violated at index {i} with right child {right}");
                    PrintNearIndex(i, 10);

                }
            }
        }


        //? FUNCTIONS FOR EXPOSED METHODS


        bool LookIfIdExists(MyId id)
        {
            if (id == null)
            {
                Debug.LogError("unity-utilities : (LookIfIdExists) CHECK NULL ID ");
                return false;
            }

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

        void RegisterToEndWithId(MyId id)
        {
            if (id == null)
            {
                Debug.LogError("unity-utilities : (RegisterToEndWithId) END NULL ID ");
                return;
            }

            for (int i = inst._invokes.Count - 1; i >= 0; i--)
            {
                var item = inst._invokes[i];
                if (item.GetId == null) continue;

                if (item.GetId.Equals(id))
                {
                    item.End();
                }
            }
        }

        void RegisterToDieWithId(MyId id)
        {
            if (id == null)
            {
                Debug.LogError("unity-utilities : (RegisterToDieWithId) KILL NULL ID ");
                return;
            }

            for (int i = inst._invokes.Count - 1; i >= 0; i--)
            {
                var item = inst._invokes[i];
                if (item.GetId == null) continue;

                if (item.GetId.Equals(id))
                {
                    item.Kill();
                }
            }
        }


        void RegisterToPauseWithId(MyId id, bool paused)
        {
            if (id == null)
            {
                Debug.LogError("unity-utilities : (RegisterToPauseWithId) PAUSE NULL ID ");
                return;
            }

            for (int i = inst._invokes.Count - 1; i >= 0; i--)
            {
                var item = inst._invokes[i];
                if (item.GetId == null) continue;

                if (item.GetId.Equals(id))
                {
                    if (paused)
                    {
                        item.Pause();
                    }
                    else
                    {
                        item.Resume();
                    }
                }
            }
        }



        //? EXPOSED INTERNAL
        internal static void Add(InvoBase thisOne)
            => inst.AddItemSorted(thisOne);
        internal static void ReorderItem(InvoBase thisOne)
            => inst.ReorderItemLocal(thisOne);





        //? EXPOSED

        public static void PauseAll(MyId id)
           => inst.RegisterToPauseWithId(id, true);
        public static void ResumeAll(MyId id)
            => inst.RegisterToPauseWithId(id, false);

        public static void KillAll(MyId id)
            => inst.RegisterToDieWithId(id);

        public static void EndAll(MyId id)
       => inst.RegisterToEndWithId(id);
        public static bool Exists(MyId id)
            => inst.LookIfIdExists(id);





    }
}
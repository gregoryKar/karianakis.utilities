using System;
using System.Collections.Generic;

namespace Karianakis.Utilities
{
    public class InvoManager
    {

        /*
            reorder item orders heapify both ways - needed ???
        */

        #region  BASICS

        static InvoManager _inst;
        public InvoManager()
        {
            _inst = this;

            if (EngineConnector.GetIsInEditor()
                    && EngineConnector.GetIsKarianakis())
            {
                _editHeapTestEnabled = true;
                EngineConnector.Log($"karianakis - init infinite invokes heap test");
            }
            else
            {
                _editHeapTestEnabled = false;
            }

        }

        List<InvoBase> _invokes = new();
        List<InvoBase> _toEvokeList = new();

        bool _heapifyUpScheduled;
        bool _heapifyDownScheduled;
        public void ScheduleHeapifyUp()
            => _heapifyUpScheduled = true;
        public void ScheduleHeapifyDown()
            => _heapifyDownScheduled = true;


        #endregion

        #region  UPDATE LOOP
        public void UpdateMe()//?------------------------
        {
            if (_invokes.Count == 0) return;

            //SortHeapifyIfScheduled_DEPRICATED();
            PrepareforeInvocations();
            ProcessAllThatNeedsToBeEvoked();

            EditTestHeapValidityAtloopEnd();
        }

        void SortHeapifyIfScheduled_DEPRICATED()
        {
            // if (_heapifyDownScheduled)
            // {
            //     _heapifyDownScheduled = false;
            //     HeapifyDown(0);
            // }
            // if (_heapifyUpScheduled)
            // {
            //     _heapifyUpScheduled = false;
            //     HeapifyUp(_invokes.Count - 1);
            // }
        }

        void PrepareforeInvocations()
        {
            _toEvokeList.Clear();
            float timeNow = EngineConnector.GetTimeNow();

            for (int i = 0; i < _invokes.Count; i++)
            {
                var invo = _invokes[i];
                if (invo.GetCanceled || invo.GetCompleted)
                {
                    RemoveItemAtIndexAndHeapify(i);
                    i--;
                    if (_invokes.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }
                else if (timeNow < invo.GetEnd)
                {
                    break;
                }
                else
                {
                    _toEvokeList.Add(invo);
                }
            }
        }
        void ProcessAllThatNeedsToBeEvoked()
        {

            for (int i = 0; i < _toEvokeList.Count; i++)
            {
                var invo = _toEvokeList[i];

                // check dead or paused or completed cause it could be triggered manually while in the invoke update loop
                if (invo.GetCanceled)
                {
                    continue;
                }
                else if (invo.GetCompleted)
                {
                    continue;
                }
                else if (invo.GetIsPaused)
                {
                    continue;
                }
                else //? MAIN CASE
                {
                    invo.InvokeMeBeforeProcessing(invo);
                    invo.ProcessAfterInvocation();
                    HeapifyDown(0);
                }
            }
        }


        void EditTestHeapValidityAtloopEnd()
        {
            if (_editHeapTestEnabled)
            {
                float timeNow = EngineConnector.GetTimeNow();
                if (timeNow >= _editHeapTestInterval
                    + _lasteditTestRegisteredTime)
                {
                    _lasteditTestRegisteredTime = timeNow;
                    TestHeapValidity();
                }
            }
        }

        #endregion


        #region EDIT TESTING FOR DEVELOPER

        bool _editHeapTestEnabled = false;
        float _lasteditTestRegisteredTime;
        const float _editHeapTestInterval = .5f;
        void TestHeapValidity()
        {
            for (int i = 0; i < _invokes.Count; i++)
            {
                int left = i * 2 + 1;
                int right = i * 2 + 2;

                if (left < _invokes.Count && _invokes[left].CompareTo(_invokes[i]) < 0)
                {

                    EngineConnector.Error($"Heap property violated at index {i} with left child {left}");
                    PrintNearIndex(i, 10);
                }

                if (right < _invokes.Count && _invokes[right].CompareTo(_invokes[i]) < 0)
                {
                    EngineConnector.Error($"Heap property violated at index {i} with right child {right}");
                    PrintNearIndex(i, 10);

                }
            }
        }

        void PrintNearIndex(int index, int distanceMax)
        {
            int Max(int a, int b) => a > b ? a : b;
            int Min(int a, int b) => a < b ? a : b;

            int start = Max(0, index - distanceMax);
            int end = Min(_invokes.Count - 1, index + distanceMax);

            for (int i = start; i <= end; i++)
            {
                if (i == index)
                {
                    EngineConnector.Error("INDEX START POINT");
                }


                EngineConnector.Log($"Index {i}: {_invokes[i].GetEnd}");
            }
        }


        public int GetInvoCount()
        {
            return _invokes.Count;
        }
        public float[] GetInvokesEndTimes()
        {
            float[] endTimes = new float[_invokes.Count];
            for (int i = 0; i < _invokes.Count; i++)
            {
                endTimes[i] = _invokes[i].GetEnd;
            }
            return endTimes;
        }


        #endregion


        #region LOW LEVEL FUNCTIONALITY

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

        void AddItem(InvoBase thisOne)
        {
            _invokes.Add(thisOne);//at end
            HeapifyUp(_invokes.Count - 1);
        }

        void RemoveItemAtIndexAndHeapify(int index)
        {
            int lastIndex = _invokes.Count - 1;
            if (index < 0 || index > lastIndex) return;

            if (index != lastIndex)
            {
                _invokes[index] = _invokes[lastIndex];
                _invokes.RemoveAt(lastIndex);

                HeapifyDown(index);
            }
            else
            {
                _invokes.RemoveAt(lastIndex);
            }
        }
        void ReorderItemLocal(InvoBase thisOne)
        {
            int index = _invokes.IndexOf(thisOne);
            if (index == -1) return;

            int parentIndex = (index - 1) / 2;
            int left = index * 2 + 1;
            int right = index * 2 + 2;

            bool hasParent = index > 0;
            bool hasLeft = left < _invokes.Count;
            bool hasRight = right < _invokes.Count;

            if (hasParent
                && _invokes[index].CompareTo(_invokes[parentIndex]) < 0)
            {
                HeapifyUp(index);
            }
            else if ((hasLeft
                && _invokes[index].CompareTo(_invokes[left]) > 0)
                ||
                    (hasRight
                && _invokes[index].CompareTo(_invokes[right]) > 0))
            {
                HeapifyDown(index);
            }
            // else, already in correct position
        }

        #endregion


        #region BASE FUNCTIONS

        bool LookIfIdExists(MyIdBase id)
        {
            if (id == null)
            {
                EngineConnector.Error("unity-utilities : (LookIfIdExists) CHECK NULL ID ");
                return false;
            }

            foreach (var item in _inst._invokes)
            {
                if (item.GetId == null) continue;

                if (MyIdBase.MainIdEqualityFunction(id, item.GetId))
                {
                    if (item.GetCanceled == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        void ForceFinishAllWithIdNowLocal(MyIdBase id)
        {
            if (id == null)
            {
                EngineConnector.Error("unity-utilities : (ForceFinishAllWithIdNowLocal) END NULL ID ");
                return;
            }


            List<InvoBase> toForceFinish = new List<InvoBase>();
            foreach (var item in _inst._invokes)
            {
                if (item.GetId == null) continue;

                if (MyIdBase.MainIdEqualityFunction(id, item.GetId))
                {
                    toForceFinish.Add(item);
                }

                for (int i = 0; i < toForceFinish.Count; i++)
                {
                    toForceFinish[i].ForceFinish();
                }

            }

        }
        void CancelAllWithIdLocalNow(MyIdBase id)
        {
            if (id == null)
            {
                EngineConnector.Error("unity-utilities : (CancelAllWithIdLocalNow) CANCEL NULL ID ");
                return;
            }

            List<InvoBase> toCancel = new List<InvoBase>();
            foreach (var item in _inst._invokes)
            {
                if (item.GetId == null) continue;

                if (MyIdBase.MainIdEqualityFunction(id, item.GetId))
                {
                    toCancel.Add(item);
                }
            }
            for (int i = 0; i < toCancel.Count; i++)
            {
                toCancel[i].Cancel();
            }
        }
        void RefreshAllPauseValueFromIdLocal(MyIdBase id)
        {
            if (id == null)
            {
                EngineConnector.Error("unity-utilities : (RefreshAllPauseValueFromIdLocal) PAUSE NULL ID ");
                return;
            }
            List<InvoBase> toRefresh = new List<InvoBase>();
            foreach (var item in _inst._invokes)
            {
                if (item.GetId == null) continue;

                if (MyIdBase.MainIdEqualityFunction(id, item.GetId))
                {
                    toRefresh.Add(item);

                }
            }
            for (int i = 0; i < toRefresh.Count; i++)
            {
                toRefresh[i].RefreshPauseFromId();
            }

        }

        void ClearAllNoCancelOrEndInvocationsLocal()
        {
            _invokes.Clear();
        }

        #endregion


        #region EXPOSED INTERNAL FUNCTIONS
        internal static void Add(InvoBase thisOne)
            => _inst.AddItem(thisOne);

        internal static void ReorderItem(InvoBase thisOne)
            => _inst.ReorderItemLocal(thisOne);

        internal static void RefreshPauseValueToAllWithId(MyIdBase id)
            => _inst.RefreshAllPauseValueFromIdLocal(id);


        #endregion


        #region EXPOSED PUBLIC FUNCTIONS


        public static void CancelAll(MyIdBase id)
            => _inst.CancelAllWithIdLocalNow(id);

        public static void ForceFinishAll(MyIdBase id)
        => _inst.ForceFinishAllWithIdNowLocal(id);
        public static bool Exists(MyIdBase id)
            => _inst.LookIfIdExists(id);

        public static void ClearAllNoKillOrEndInvocations()
            => _inst.ClearAllNoCancelOrEndInvocationsLocal();

        public static void PrintAllId()
        {

            EngineConnector.Error($" TOTAL INVOKES : " + _inst._invokes.Count);
            foreach (var item in _inst._invokes)
            {
                if (item.GetId == null)
                {
                    EngineConnector.Error("ID NULL : INVO HAS : " + item.GetHashCode());
                    continue;
                }

                if (item.GetId is MyId myId)
                {
                    EngineConnector.Error("ID : " + myId._id);
                    EngineConnector.Error("ID HAS = " + myId.GetHashCode());
                }
                else
                {
                    EngineConnector.Error("ID : NOT MyId " + item.GetId.GetHashCode());
                    EngineConnector.Error("ID HAS = " + item.GetId.GetHashCode());
                }
            }
        }

        #endregion




    }
}
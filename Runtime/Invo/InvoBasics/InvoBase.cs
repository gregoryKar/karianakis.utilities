using System;
using UnityEngine;

namespace Karianakis.Utilities
{
    public abstract class InvoBase : IComparable<InvoBase>
    {
        MyId _id;
        float _delay;
        float _savedDelay;
        float _end;
        int _repeatsMax;
        int _iterationIndex;
        bool _infinite;

        bool _DEAD;

        //? INTERNAL STATUS
        bool _completed;
        bool _isPaused;

        //? BOOL ORDERS
        bool _shceduledToPause;
        bool _shceduledToResume;
        bool _shceduledToDie;
        bool _shceduledToEnd;


        Action _endAction;
        Action _deathAction;

        internal InvoBase(
           float delay,
           int repeats,
           MyId id)
        {
#if UNITY_EDITOR
            if (Application.isEditor && Application.isPlaying is false)
            {
                Debug.LogError("InvoBase should not be created in edit mode");
                return;
            }
#endif

            _end = MyTime.now + delay;

            _delay = delay;
            _repeatsMax = repeats;
            _id = id;

            _infinite = IsInfiniteRepeats(repeats);

            InvoManager.Add(this);
        }


        //? GETTERS INTERNAL 
        internal const int _infiniteRepeats = -1;
        internal bool IsInfiniteRepeats(int repeats)
            => repeats == _infiniteRepeats;
        internal const float _defaultPausedTime = float.MaxValue;
        internal float GetEnd => _end;
        internal bool GetCompleted => _completed;
        internal MyId GetId => _id;

        internal bool GetScheduledToPause => _shceduledToPause;
        internal bool GetScheduledToResume => _shceduledToResume;
        internal bool GetScheduledToDie => _shceduledToDie;
        internal bool GetScheduledToEnd => _shceduledToEnd;



        //? SETTERS INTERNAL
        internal bool MarkAsDead()
            => _DEAD = true;
        internal void ResetScheduledToPauseResume()
        {
            _shceduledToPause = false;
            _shceduledToResume = false;
        }
        internal void SetIdInternal(MyId id)
                 => _id = id;
        internal void SetEndActionInternal(Action endAction)
            => _endAction = endAction;
        internal void SetDeathActionInternal(Action deathAction)
            => _deathAction = deathAction;
        internal void SetEndTime(float endTime)
          => _end = endTime;
        internal void SetPaused(bool isPaused)
            => _isPaused = isPaused;


        //? INTERNAL FUNCTIONS
        internal void OvverideEndTimeAndReorder(float delayFromNow)
        {
            _end = MyTime.now + delayFromNow;
            InvoManager.ReorderItem(this);
        }

        internal void TriggerDeathAction()
        {
            _deathAction?.Invoke();
        }
        internal void TriggerEndAction()
        {
            _endAction?.Invoke();
        }
        internal void SaveEndDifferenceFromNow()
        {
            _savedDelay = MyTime.now - _end;
        }
        internal void SetEndFromSavedEndFromNow()
        {
            _end = MyTime.now + _savedDelay;
        }



        /// <summary>
        ///    IS BEFORE PROCESS  METHOD
        /// </summary>
        internal abstract void InvokeMeBeforeProcessing(InvoBase _me);


        //? MASTER PROCESS ------------------------ ------------------------- 
        /// <summary>
        ///    IS AFTER INVOKE ME METHOD
        /// </summary>
        internal void ProcessAfterInvocation()
        {
            _iterationIndex++;

            if (_infinite is false && _iterationIndex >= _repeatsMax)
            {
                _completed = true;
                MarkAsDead();
                TriggerEndAction();
            }
            else
            {
                _end = MyTime.now + _delay;
            }
        }




        //? EXPOSED GETTERS
        public int GetIterationIndex => _iterationIndex;
        public int GetRepeatsLeft => _repeatsMax - _iterationIndex - 1;
        public float GetDelay => _delay;
        public bool GetIsPaused => _isPaused;

        //? EXPOSED FUNCTIONS
        public void Pause()
        {
            if (_DEAD)
            {
#if PACKAGE_EDITOR
                Debug.LogError("unity-utilities : (INVOBASE) ATTEMPT PAUSE - DEAD DEAD DEAD");
#endif
            }
            else if (_isPaused)
            {
#if PACKAGE_EDITOR
                Debug.LogError("unity-utilities : (INVOBASE) ATTEMPT PAUSE - INVO ALREADY PAUSED");
#endif
            }
            else
            {
                SaveEndDifferenceFromNow();
                ResetScheduledToPauseResume();
                _shceduledToPause = true;
                OvverideEndTimeAndReorder(-1f);
            }
        }

        public void Resume()
        {
            if (_DEAD)
            {
#if PACKAGE_EDITOR
                Debug.LogError("unity-utilities : (INVOBASE) ATTEMPT RESUME - DEAD DEAD DEAD");
#endif
            }
            else if (_isPaused == false)
            {
#if PACKAGE_EDITOR
                Debug.LogError("unity-utilities : (INVOBASE) ATTEMPT RESUME - INVO NOT PAUSED");
#endif
            }
            else
            {
                ResetScheduledToPauseResume();
                _shceduledToResume = true;
                OvverideEndTimeAndReorder(-1f);
            }
        }

        public void Kill()
        {
            if (_DEAD)
            {

#if PACKAGE_EDITOR
                Debug.LogError("unity-utilities : (INVOBASE) ATTEMPT KILL - DEAD DEAD DEAD");
#endif
            }
            else if (_shceduledToDie == true)
            {
#if PACKAGE_EDITOR
                Debug.LogError("unity-utilities : (INVOBASE) ATTEMPT KILL - INVO ALREADY KILLED");
#endif
            }
            else
            {
                _shceduledToDie = true;
                OvverideEndTimeAndReorder(-1f);
            }
        }

        public void End()
        {
            if (_DEAD)
            {
#if PACKAGE_EDITOR  
                Debug.LogError("unity-utilities : (INVOBASE) ATTEMPT END - DEAD DEAD DEAD");
#endif
            }
            else if (_shceduledToEnd == true)
            {
#if PACKAGE_EDITOR
                Debug.LogError("unity-utilities : (INVOBASE) ATTEMPT END - INVO ALREADY ENDED");
#endif
            }
            else
            {
                _shceduledToEnd = true;
                OvverideEndTimeAndReorder(-1f);
            }
        }



        public int CompareTo(InvoBase other)
        {
            return GetEnd.CompareTo(other.GetEnd);
        }



        //? BUILDER METHODS FOR ALL DO 
        public InvoBase SetDelay(float delay)
        {
            if (_DEAD)
            {
                Debug.LogError("unity-utilities : (INVOBASE) ATTEMPT SET DELAY - DEAD DEAD DEAD");
            }
            _delay = delay;
            return this;
        }
        public InvoBase SetStartDelay(float startDelay)
        {
            OvverideEndTimeAndReorder(startDelay);
            return this;
        }
        public InvoBase SetId(MyId id)
        {
            SetIdInternal(id);
            return this;
        }
        public InvoBase SetEndAction(Action endAction)
        {
            SetEndActionInternal(endAction);
            return this;
        }
        public InvoBase SetDeathAction(Action deathAction)
        {
            SetDeathActionInternal(deathAction);
            return this;
        }
        public InvoBase SetDeathOrEndAction(Action deathAction)
        {
            SetDeathActionInternal(deathAction);
            SetEndActionInternal(deathAction);
            return this;
        }

    }
}
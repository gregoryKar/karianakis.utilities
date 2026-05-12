using System;

namespace Karianakis.Utilities
{
    [Serializable]
    public abstract class InvoBase : IComparable<InvoBase>
    {

        /*
         
        */

        #region BASICS

        MyIdBase _idForbidden;
        MyIdBase Id
        {
            get
            {
                return _idForbidden;
            }
            set
            {
                _idForbidden = value;
                if (value != null)
                {
                    Paused = _idForbidden.GetIsPaused;
                }

            }
        }


        float _delay;
        float _savedDelay;
        float _end;
        int _repeatsMax;
        int _iterationIndex;
        bool _infinite;


        //?  STATUS
        bool _canceled;
        bool _finished;



        Action _endAction;
        Action _cancelAction;


        internal const int _infiniteRepeats = -1;
        bool IsInfiniteRepeats(int repeats)
          => repeats == _infiniteRepeats;


        internal InvoBase(
           float delay,
           int repeats,
           MyId id)
        {
            _end = MyTime.GetNow + delay;

            _delay = delay;
            _repeatsMax = repeats;
            Id = id;


            _infinite = IsInfiniteRepeats(repeats);

            InvoManager.Add(this);
        }


        #endregion

        #region  PAUSE - RESUME



        bool _pausedForbidden = false;
        public bool GetIsPaused => _pausedForbidden;


        bool Paused
        {
            get
            {
                return _pausedForbidden;
            }
            set
            {
                if (GetCanceledOrCompleted)
                {
                    EngineConnector.Error($"(INVOBASE) ATTEMPT SET PAUSE {value} - A DEAD INVOKE");
                }
                else if (value != _pausedForbidden)
                {
                    _pausedForbidden = value;
                    if (value == true)
                    {
                        SaveEndDifferenceFromNow();
                        OvverideEndTimeAndReorder(float.MaxValue);
                    }
                    else
                    {
                        SetEndFromSavedEndFromNow();
                        InvoManager.ReorderItem(this);
                    }
                }
            }
        }




        internal void RefreshPauseFromId()
            => Paused = Id.GetIsPaused;

        public void OverrideSetPauseIdValue(bool paused)
        {
            if (Id != null)
            {
                EngineConnector.Error("PAUSED IS ID DEPENDANT - attempt to override pause value directly while ID present , DANGEROUS BEHAVIOUR , id can possible ovveride the value later");
            }
            _pausedForbidden = paused;
        }





        #endregion

        #region GETTERS SETTERS

        internal float GetEnd => _end;
        internal bool GetCompleted => _finished;
        internal MyIdBase GetId => Id;
        internal bool GetCanceled => _canceled;


        public int GetIterationIndex => _iterationIndex;
        public bool GetIsLastIteration => _infinite == false && _iterationIndex == _repeatsMax - 1;
        public int GetRepeatsLeft => _repeatsMax - _iterationIndex - 1;
        public float GetDelay => _delay;
        public bool GetCanceledOrCompleted => _canceled || _finished;
        public float GetSavedTime => _savedDelay;
        public float GetEndTime => _end;


        internal void SetIdInternal(MyIdBase id)
            => Id = id;
        internal void SetFinishActionInternal(Action endAction)
            => _endAction = endAction;
        internal void SetCancelActionInternal(Action canelAction)
            => _cancelAction = canelAction;


        #endregion



        //? INTERNAL FUNCTIONS
        internal void OvverideEndTimeAndReorder(float delayFromNow)
        {
            _end = MyTime.GetNow + delayFromNow;
            InvoManager.ReorderItem(this);
        }

        internal void TriggerDeathAction()
        {
            _cancelAction?.Invoke();
        }
        internal void TriggerEndAction()
        {
            _endAction?.Invoke();
        }
        internal void SaveEndDifferenceFromNow()
        {
            _savedDelay = _end - MyTime.GetNow;
        }
        internal void SetEndFromSavedEndFromNow()
        {
            _end = MyTime.GetNow + _savedDelay;
        }


        internal abstract void InvokeMeBeforeProcessing(InvoBase _me);


        //? MASTER PROCESS ------------------------ ------------------------- 
        internal void ProcessAfterInvocation()
        {
            _iterationIndex++;

            if (_infinite is false && _iterationIndex >= _repeatsMax)
            {
                _finished = true;
                TriggerEndAction();
            }
            else
            {
                _end = MyTime.GetNow + _delay;
            }
        }








        #region EXPOSED MAIN FUNCTIONS

        public void Cancel()
        {
            if (GetCanceledOrCompleted)
            {
                EngineConnector.Error("(INVOBASE) ATTEMPT KILL - A DEAD INVOKE");
            }
            else
            {
                _canceled = true;
                OvverideEndTimeAndReorder(-1f);
                TriggerDeathAction();
            }
        }

        public void ForceFinish()
        {
            if (GetCanceledOrCompleted)
            {
                EngineConnector.Log("(INVOBASE) ATTEMPT END - A DEAD INVOKE");
            }
            else
            {
                _finished = true;
                OvverideEndTimeAndReorder(-1f);
                TriggerEndAction();
            }
        }

        public void SetDelayForceChangeNow(float delay)
            => OvverideEndTimeAndReorder(_delay);



        #endregion


        #region BUILDER METHODS FOR ALL DO 

        /// <summary>
        /// Sets the delay for the next and all subsequent iterations. Does not change the current end time. Use SetDelayForceChangeNow to change the current end time as well.
        /// </summary>
        public InvoBase SetDelay(float delay)
        {
            _delay = delay;
            return this;
        }
        public InvoBase SetStartDelay(float startDelay)
        {
            OvverideEndTimeAndReorder(startDelay);
            return this;
        }
        public InvoBase SetId(MyIdBase id)
        {
            SetIdInternal(id);
            return this;
        }
        public InvoBase SetFinishAction(Action endAction)
        {
            SetFinishActionInternal(endAction);
            return this;
        }
        public InvoBase SetCancelAction(Action cancelAction)
        {
            SetCancelActionInternal(cancelAction);
            return this;
        }
        public InvoBase SetCancelOrFinishAction(Action cancelOrFinishAction)
        {
            SetCancelActionInternal(cancelOrFinishAction);
            SetFinishActionInternal(cancelOrFinishAction);
            return this;
        }

        #endregion

        public int CompareTo(InvoBase other)
        {
            return GetEnd.CompareTo(other.GetEnd);
        }


    }
}
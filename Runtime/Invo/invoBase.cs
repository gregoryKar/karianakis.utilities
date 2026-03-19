


//todo instead of float time , myTime variable that can just set to gameTime
// so no need to seperate in process logic

using System;
using UnityEngine;

namespace Karianakis.Utilities
{

    public abstract class InvoBase : IComparable<InvoBase>
    {

        MyId _id;
        float _delay;
        float _end;
        int _repeatsLeft;
        int _iterationIndex;
        bool _infinite;
        bool _killMe;

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
            _repeatsLeft = repeats;
            _id = id;

            _infinite = IsInfiniteRepeats(repeats);

            InvoManager.Add(this);
        }


        internal const int _infiniteRepeats = -1;
        internal bool IsInfiniteRepeats(int repeats)
                  => repeats == _infiniteRepeats;
        internal float GetEnd => _end;
        internal bool GetKillMe => _killMe;
        internal MyId GetId => _id;

        internal void OvverideCurrentEndTime(float delay)
        {
            _end = MyTime.now + delay;
            InvoManager.ReorderItem(this);
        }

        internal void SetIdInternal(MyId id)
                 => _id = id;

        internal void SetEndActionInternal(Action endAction)
            => _endAction = endAction;

        internal void SetDeathActionInternal(Action deathAction)
            => _deathAction = deathAction;


        internal void Process()
        {
            _repeatsLeft--;
            _iterationIndex++;

            if (_infinite is false && _repeatsLeft < 1)
            {
                _killMe = true;
                _endAction?.Invoke();
            }
            else
            {
                _end = MyTime.now + _delay;
            }
        }


        internal abstract void InvokeMe(InvoBase _me);


        //? EXPOSED
        public int GetIterationIndex => _iterationIndex;
        public int GetRepeatsLeft => _repeatsLeft;
        public float GetDelay => _delay;


        /// <summary>
        /// sets the delay and ignores the current time
        /// so it needs to get executed to take effect
        /// if called from withing the invocation of the invo
        /// the the this will be the new delay
        /// if called at random time need to wait for previous
        /// time left
        /// <param name="delay"></param>
        public void SetDelay(float delay)
            => _delay = delay;
        public void KillMe()
        {
            _killMe = true;
            _deathAction?.Invoke();
        }


        public int CompareTo(InvoBase other)
        {
            return GetEnd.CompareTo(other.GetEnd);
        }
    }
}
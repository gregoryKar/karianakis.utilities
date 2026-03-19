
using UnityEngine;
using System;



// create instance using it for a time stamp
// why comparable ?? ??

namespace Karianakis.Utilities
{

    [Serializable]
    public struct TimeStamp : IComparable<TimeStamp>
    {



        float _time;
        public float GetTime => _time;



        //SETTERS
        public void SetCustomTime(float time) => _time = time;
        public void SetNow() => _time = MyTime.now;
        public void SetFromNow(float after) => _time = MyTime.now + after;
        public void SetAfterPoint(float point, float after) 
            => SetCustomTime(point + after);


        public bool HasThatAmountPassed(float amount) => MyTime.now > _time + amount;

        //COMPARISONS
        public bool MommentPassed() 
            => MyTime.now > _time;
        // when momment has passed the time will be bigger like time = 2 seconds second , the momment 1 seconds
        public bool Isbefore(TimeStamp other) => _time < other._time;
        public bool IsAfter(TimeStamp other) => _time > other._time;




        public void DebugMyTime() => Debug.Log("timeStamp ==" + _time);


        public int CompareTo(TimeStamp other) => _time.CompareTo(other._time);
       








    }

}
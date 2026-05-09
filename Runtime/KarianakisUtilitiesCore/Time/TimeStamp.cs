
//using UnityEngine;
using System;
namespace Karianakis.Utilities
{

    [Serializable]
    public struct TimeStamp : IComparable<TimeStamp>
    {

        float _time;
        public float GetTime => _time;



        //SETTERS
        public void SetCustomTime(float time) => _time = time;
        public void SetNow() => _time = MyTime.GetNow;
        public void SetFromNow(float after) => _time = MyTime.GetNow + after;
        public void SetAfterPoint(float point, float after)
            => SetCustomTime(point + after);


        public bool HasThatAmountPassed(float amount) => MyTime.GetNow > _time + amount;

        //COMPARISONS
        public bool MommentPassed()
            => MyTime.GetNow > _time;
        // when momment has passed the time will be bigger like time = 2 seconds second , the momment 1 seconds
        public bool Isbefore(TimeStamp other) => _time < other._time;
        public bool IsAfter(TimeStamp other) => _time > other._time;




        public void DebugMyTime() 
            => EngineConnector.Log("timeStamp ==" + _time);


        public int CompareTo(TimeStamp other) => _time.CompareTo(other._time);









    }

}
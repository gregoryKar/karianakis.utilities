
using UnityEngine;
using System;



// create instance using it for a time stamp
// why comparable ?? ??

namespace kar_main_utils
{

    [Serializable]
    public struct timeStamp : IComparable<timeStamp>
    {


        public float testVariable;
        //public timeStamp(float after = 0) { _time = myTime.now + after; }

        float _time;//! FLOAT DECIDED
        public float getTime => _time;  



        //SETTERS
        public void setCustom(float time) => _time = time;
        public void setNow() => _time = myTime.now;
        public void setAfter(float after) => _time = myTime.now + after;
        public void setAfterPoint(float point, float after) => setCustom(point + after);



        //COMPARISONS
        public bool mommentPassed() => myTime.now > _time;
        // when momment has passed the time will be bigger like time = 2 seconds second , the momment 1 seconds
        public bool isbefore(timeStamp other) => _time < other._time;
        public bool isAfter(timeStamp other) => _time > other._time;




        public void debugMyTime() => Debug.Log("timeStamp ==" + _time);


        public int CompareTo(timeStamp other) => _time.CompareTo(other._time);
        // what possible uses does this have









    }

}
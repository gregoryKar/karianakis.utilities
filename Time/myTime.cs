
using UnityEngine;
using System;

//! A SINGLE MENTION SO FAR OF ENGINE TIME VARIABLE
// ALL OTHER GET TO THIS .. YOU WANT TO CHANGE IT 
// CHANGE ONE LINE AND DONE .. .. 




namespace kar_main_utils
{

    public static class myTime
    {


        public static float now => Time.timeSinceLevelLoad;

        public static void debugTime() => Debug.Log("NOW=" + now);

        public static bool mommentPassed(float momment) => momment > now;


    }

}

//using UnityEngine;
using System;
namespace Karianakis.Utilities
{

    public static class MyTime
    {

        public static float GetNow => EngineConnector.GetTimeNow();

        public static bool HasMommentPassed(float momment) => momment < GetNow;


    }

}

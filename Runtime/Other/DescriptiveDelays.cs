using System;
using UnityEngine;

#if UNITY_EDITOR && ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Karianakis.Utilities
{
    [Serializable]
    public class DescriptiveDelays
    {

#if UNITY_EDITOR && ODIN_INSPECTOR
        [ListDrawerSettings(ShowFoldout = true)]
#endif
        [SerializeField] DescriptiveDelayItem[] _delays;

        public float[] GetDelays()
        {
            float[] delays = new float[_delays.Length];
            for (int i = 0; i < _delays.Length; i++)
            {
                delays[i] = _delays[i].GetDelay;
            }
            return delays;
        }


        [Serializable]
        class DescriptiveDelayItem
        {
            [SerializeField] float Delay;
            [SerializeField] string Description;

            public float GetDelay => Delay;
        }
    }
}
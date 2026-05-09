using System;

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
        DescriptiveDelayItem[] _delays;

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
            float Delay;
            string Description;

            public float GetDelay => Delay;
        }
    }
}
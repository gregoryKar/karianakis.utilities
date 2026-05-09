using System;
using System.Collections.Generic;

namespace Karianakis.Utilities
{
    public class InvoGroup : InvoBase
    {
        List<Action> _actions = new List<Action>();
        List<float> _delays = new List<float>();

        Action _everyTimeAfterAction;
        Action _everyTimeBeforeAction;

        internal InvoGroup(float startDelay)
        : base(0, _infiniteRepeats, null)
        {
            OvverideEndTimeAndReorder(startDelay);
        }
        public static InvoGroup Create(float startDelay) => new InvoGroup(startDelay: startDelay);


        internal override void InvokeMeBeforeProcessing(InvoBase _me)
        {


// #if UNITY_EDITOR
//             if (_actions.Count != _delays.Count)
//             {
//                 GD.PrintErr($"InvoGroup: actions and delays count mismatch {_actions.Count} actions and {_delays.Count} delays");
//                 End();
//                 return;
//             }
// #endif

            if (GetIterationIndex < _actions.Count)
            {
                _everyTimeBeforeAction?.Invoke();

                _actions[GetIterationIndex]?.Invoke();

                _everyTimeAfterAction?.Invoke();

                if (GetIterationIndex + 1 < _delays.Count)
                {
                    SetDelay(_delays[GetIterationIndex + 1]);
                }

                if (GetIterationIndex == _actions.Count - 1)
                {
                    ForceFinish();
                }

            }
          
        }

        public InvoGroup SetDelayArray(float[] delays)
        {
            _delays = new List<float>(delays);
            OvverideEndTimeAndReorder(delays[0]);
            return this;
        }

        public InvoGroup ThenDo(Action action)
        {
            _actions.Add(action);
            return this;
        }
        public InvoGroup DoEveryTimeAFTER(Action action)
        {
            _everyTimeAfterAction = action;
            return this;
        }
        public InvoGroup DoEveryTimeBEFORE(Action action)
        {
            _everyTimeBeforeAction = action;
            return this;
        }

     


    }

}
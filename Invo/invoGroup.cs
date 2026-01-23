
using System;
using UnityEngine;



namespace kar_main_utils
{








    public class InvoGroup : invoBase
    {



        Action[] _actionArray;
        float[] _delayArray;

        int _counter = 0;

        public InvoGroup((Action, float)[] array, float delay = 0) : base(delay: delay)
        {
            _actionArray = new Action[array.Length];//
            _delayArray = new float[array.Length];

            for (int i = 0; i < array.Length; i++)
            {
                _actionArray[i] = array[i].Item1;
                _delayArray[i] = array[i].Item2;
            }

            _repeatsLeft = array.Length;

        }


        public override void invokeMe(invoBase _me)
        {


            if (_counter >= _actionArray.Length) { _killMe = true; return; }

            _actionArray[_counter].Invoke();
            _me._end = myTime.now + _delayArray[_counter];

           // Debug.LogWarning("TIME = " + myTime.now + "__" + _delayArray[_counter] + "__end " + _me._end);
            // Debug.LogWarning("INVOKE GROUP counter = " + _counter + " _delay = " + _delayArray[_counter]);

            _counter++;


        }
        public override void process()
        {
            //!base.process(); NOP
        }

        // private invo(Action action, float delay, int repeatsLeft, bool infinite = false) : base(delay: delay, repeats: repeatsLeft) { _action = action; invoManager.add(this); }



        // public static void simple(Action action, float delay) => new invo(action, delay, 0);
        // public static void repeat(Action action, float delay, int repeat) => new invo(action, delay, repeat);

        // public static invo infinite(Action action, float delay) =>
        // new invo(action, delay, 0, infinite: true);


        // public override void invokeMe(invoBase _me) => _action.Invoke();







    }



}
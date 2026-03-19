
using System;


namespace Karianakis.Utilities
{



    public class InvoBuilder : InvoBase
    {

        internal Action _action;


        internal InvoBuilder(
            Action action,
            float delay,
            int repeats)
               : base(delay, repeats, null)
        { _action = action; }


        internal override void InvokeMe(InvoBase _me) => _action.Invoke();




        //? EXPOSED
        public InvoBuilder SetStartDelay(float startDelay)
        {
            OvverideCurrentEndTime(startDelay);
            return this;
        }

        public InvoBuilder SetId(MyId id)
        {
            SetIdInternal(id);
            return this;
        }


        public InvoBuilder SetEndAction(Action endAction)
        {
            SetEndActionInternal(endAction);
            return this;
        }
        public InvoBuilder SetDeathAction(Action deathAction)
        {
            SetDeathActionInternal(deathAction);
            return this;
        }
        public InvoBuilder SetDeathOrEndAction(Action deathAction)
        {
            SetDeathActionInternal(deathAction);
            SetEndActionInternal(deathAction);
            return this;
        }

    }
}

using System;
using UnityEngine;


namespace Karianakis.Utilities
{
    public class invoAdvancedBuilder : InvoBase
    {


        internal Action<invoAdvancedBuilder> _action;


        internal invoAdvancedBuilder(Action<invoAdvancedBuilder> action, float delay, int repeatsLeft) : base(delay, repeatsLeft, null) => _action = action;


        internal override void InvokeMe(InvoBase _me) => _action.Invoke((invoAdvancedBuilder)_me);


        //? EXPOSED
        public invoAdvancedBuilder SetStartDelay(float startDelay)
        {
            OvverideCurrentEndTime(startDelay);
            return this;
        }

        public invoAdvancedBuilder SetId(MyId id)
        {
            SetIdInternal(id);
            return this;
        }
        public invoAdvancedBuilder SetEndAction(Action endAction)
        {
            SetEndActionInternal(endAction);
            return this;
        }
        public invoAdvancedBuilder SetDeathAction(Action deathAction)
        {
            SetDeathActionInternal(deathAction);
            return this;
        }
        public invoAdvancedBuilder SetDeathOrEndAction(Action deathAction)
        {
            SetDeathActionInternal(deathAction);
            SetEndActionInternal(deathAction);
            return this;
        }


    }
}


using System;
using UnityEngine;


namespace Karianakis.Utilities
{
    public class invoAdvancedBuilder : InvoBase
    {


        internal Action<invoAdvancedBuilder> _action;


        internal invoAdvancedBuilder(Action<invoAdvancedBuilder> action, float delay, int repeatsLeft) : base(delay, repeatsLeft, null) => _action = action;


        internal override void InvokeMeBeforeProcessing(InvoBase _me) => _action.Invoke((invoAdvancedBuilder)_me);


     
    }
}

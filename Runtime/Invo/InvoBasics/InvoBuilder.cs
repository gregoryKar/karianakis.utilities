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


        internal override void InvokeMeBeforeProcessing(InvoBase _me) => _action.Invoke();


    }
}
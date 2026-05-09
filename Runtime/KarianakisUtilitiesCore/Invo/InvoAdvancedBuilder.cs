using System;

namespace Karianakis.Utilities
{
    [Serializable]
    public class InvoAdvancedBuilder : InvoBase
    {


        internal Action<InvoAdvancedBuilder> _action;


        internal InvoAdvancedBuilder(Action<InvoAdvancedBuilder> action, float delay, int repeatsLeft) : base(delay, repeatsLeft, null) => _action = action;


        internal override void InvokeMeBeforeProcessing(InvoBase _me) => _action.Invoke((InvoAdvancedBuilder)_me);


     
    }
}

using System;
namespace Karianakis.Utilities
{
    public class InvoAdvanced
    {
        public static InvoAdvancedBuilder Simple
             (Action<InvoAdvancedBuilder> action, float delay)
                 => new InvoAdvancedBuilder(action, delay, 0);

        public static InvoAdvancedBuilder Repeat
            (Action<InvoAdvancedBuilder> action, float delay, int repeat)
                => new InvoAdvancedBuilder(action, delay, repeat);

        public static InvoAdvancedBuilder Infinite
            (Action<InvoAdvancedBuilder> action, float delay)
                => new InvoAdvancedBuilder(action, delay, InvoBase._infiniteRepeats);

    }
}
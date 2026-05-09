using System;
namespace Karianakis.Utilities
{
    public class Invo
    {
        public static InvoBuilder Simple(Action action, float delay)
            => new InvoBuilder(action, delay, 0);

        public static InvoBuilder Repeat(Action action, float delay, int repeat)
            => new InvoBuilder(action, delay, repeat);

        public static InvoBuilder Infinite(Action action, float delay)
            => new InvoBuilder(action, delay, InvoBase._infiniteRepeats);


    }
}
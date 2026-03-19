



using System;

namespace Karianakis.Utilities
{
    //! need to seperate ids ??? like touch id and button id ??

    public abstract class MyIdBase
    {
        public abstract void KillMe();


        internal abstract bool ContainsIntId(int number);
        protected abstract bool OverlapsInternal(MyIdBase other);

        public bool Overlaps(MyIdBase other)
        {
            if (other == null) return false;
            return OverlapsInternal(other);
        }



    }
}
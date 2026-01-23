



using System;

namespace kar_main_utils
{
    //! need to seperate ids ??? like touch id and button id ??

    public abstract class myIdBase : IEquatable<myIdBase>
    {




        // public abstract idType getIdType();
        public abstract bool containsInt(int number);
        public abstract void killAll();// => invoManager.killAll(this);
        public abstract bool Equals(myIdBase other);



        
        // public enum idType
        // { simple, none }

    }
}
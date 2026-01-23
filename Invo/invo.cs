
using System;


namespace kar_main_utils
{



    public class invo : invoBase // IComparable<invo>, IEquatable<invo>
    {

        public Action _action;

        private invo(Action action, float delay, int repeatsLeft, bool infinite = false, myId id = null, float startDelay = -1) : base(delay: delay, repeats: repeatsLeft, infinite: infinite, id: id, startDelay: startDelay) { _action = action; }



        public static void simple(Action action, float delay, myId id = null) => new invo(action, delay, 0, id: id);
        public static void repeat(Action action, float delay, int repeat, myId id = null, float startDelay = -1) => new invo(action, delay, repeat, id: id, startDelay: startDelay);

        public static invo infinite(Action action, float delay, myId id = null, float startDelay = -1) =>
        new invo(action, delay, 0, infinite: true, id: id , startDelay: startDelay);


        public override void invokeMe(invoBase _me) => _action.Invoke();


        // public int CompareTo(invo other) return _endTime.CompareTo(other._endTime);
        // public bool Equals(invo other) return this == other;




    }



}
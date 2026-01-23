


//todo instead of float time , myTime variable that can just set to gameTime
// so no need to seperate in process logic

namespace kar_main_utils
{

    public abstract class invoBase
    {


        public myId _id;
        public float _delay, _end;
        public int _repeatsLeft;
        public bool _infinite, _killMe;


        public abstract void invokeMe(invoBase _me);


        public invoBase(float delay = 0, int repeats = 0, bool infinite = false, myId id = null, float startDelay = -1)
        {
            if (startDelay < 0) _end = myTime.now + delay;
            else _end = myTime.now + startDelay;


            _delay = delay;
            _repeatsLeft = repeats;
            _infinite = infinite;

            _id = id;

            _infinite = repeats == -1 || infinite;

            invoManager.add(this);
        }

        public virtual void process()
        {
            _repeatsLeft--;

            if (_infinite is false && _repeatsLeft < 1) _killMe = true;
            else _end = myTime.now + _delay;

        }

        public void setDelay(float delay) => _delay = delay;

    }
}
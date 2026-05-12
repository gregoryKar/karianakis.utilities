using System;

namespace Karianakis.Utilities
{
    /*
    equals , == != all use the int id check
    if you want to check if they are the same reference use StrictReferenceEquals
    */
    [Serializable]
    public abstract class MyIdBase
    {


        protected bool _pausedLocalForbidden = false;


        public void SetPaused(bool paused)
        {
            if (paused == _pausedLocalForbidden)
            {
                EngineConnector.Log($"(INVOBASE) ATTEMPT SET PAUSE {paused} - INVO ALREADY IN THIS STATE");
            }
            else
            {
                _pausedLocalForbidden = paused;
                InvoManager.RefreshPauseValueToAllWithId(this);
            }

        }
        public virtual bool GetIsPaused => _pausedLocalForbidden;

        bool _haveLinkedItems = false;
        internal bool HaveLinkedItems => _haveLinkedItems;
        internal void NotifyHaveLinkedItems()
            => _haveLinkedItems = true;
        internal void NotifyClearedLinkedItems()
        => _haveLinkedItems = false;




        static bool MainNonNullIdEqualityFunction(MyIdBase thisId, MyIdBase otherId)
        {
            //ReferenceEquals(thisId, otherId))
            return thisId.OverlapsInternal(otherId);
        }

        internal static bool MainIdEqualityFunction(MyIdBase thisId, MyIdBase otherId)
        {

            if (ReferenceEquals(thisId, null) || ReferenceEquals(otherId, null))
            {
                if (ReferenceEquals(thisId, null) && ReferenceEquals(otherId, null))
                {
                    return true;
                }
                else return false;
            }
            else
            {
                return MainNonNullIdEqualityFunction(thisId, otherId);
            }
        }


        public override bool Equals(object obj)
        {
            if (obj is MyIdBase other)
            {
                return MainIdEqualityFunction(this, other);
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode() => base.GetHashCode();


        public static bool operator ==(MyIdBase left, MyIdBase right)
        {
            return MainIdEqualityFunction(left, right);
        }

        public static bool operator !=(MyIdBase left, MyIdBase right)
            => !(left == right);




        internal abstract bool ContainsIntId(int number);
        protected abstract bool OverlapsInternal(MyIdBase other);

        public bool Overlaps(MyIdBase other)
        {
            if (other == null) return false;
            return OverlapsInternal(other);
        }


        public bool StrictReferenceEquals(MyIdBase otherId)
        {
            return ReferenceEquals(this, otherId);
        }
    }
}
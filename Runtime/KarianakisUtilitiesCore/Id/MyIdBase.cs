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



        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (obj is MyIdBase other)
                return Overlaps(other);
            return false;
        }
        public override int GetHashCode() => base.GetHashCode();


        public static bool operator ==(MyIdBase left, MyIdBase right)
        {
            //without this check if both are null it would return false instead of true
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Overlaps(right);
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
            if (otherId == null)
            {
                return false;
            }
            return ReferenceEquals(this, otherId);
        }
    }
}
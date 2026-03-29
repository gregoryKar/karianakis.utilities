namespace Karianakis.Utilities
{
    /*
    equals , == != all use the int id check
    if you want to check if they are the same reference use StrictReferenceEquals
    */
    public abstract class MyIdBase
    {
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
        {
            return !(left == right);
        }

        public abstract void KillMe();


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
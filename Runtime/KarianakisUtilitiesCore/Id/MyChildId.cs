using System;

namespace Karianakis.Utilities
{
    public class MyChildId : MyIdBase
    {

        //! CHILD == FATHER FALSE
        //! FATHER == CHILD TRUE
        MyId _childId;
        MyId _fatherId;
        public MyChildId(MyId father)
        {
            _childId = new MyId();
            _fatherId = father;
        }

        internal MyId GetChildId => _childId;
        internal MyId GetParentId => _fatherId;

        public override bool GetIsPaused => base.GetIsPaused || _fatherId.GetIsPaused;

        /// <summary>
        /// kills ONLY THE CHILD the father needs to be killed separately
        /// </summary>
        public void KillAllWithChildId()
            => InvoManager.CancelAll(_childId);

        internal override bool ContainsIntId(int number)
            => _childId.ContainsIntId(number) ||
            _fatherId.ContainsIntId(number);

        protected override bool OverlapsInternal(MyIdBase other)
        {
            if (other.ContainsIntId(_childId._id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
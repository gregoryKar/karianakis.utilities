



using System;
using UnityEngine;

namespace Karianakis.Utilities
{

    public class MyId : MyIdBase //: IEquatable<myId>, IEquatable<int>
    {


        static int _idCounter;
        int _idForbidden;
        public int _id
        {
            get
            {
                return _idForbidden;
            }
            private set
            {
                _idForbidden = value;
            }
        }
        public MyId()
        {
            _idCounter++;
            _id = _idCounter;
        }



        public override void KillMe() => InvoManager.KillAll(this);
      
        internal override bool ContainsIntId(int number) 
            => _id == number;
        protected override bool OverlapsInternal(MyIdBase other)
        {
            if (other.ContainsIntId(_id))
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
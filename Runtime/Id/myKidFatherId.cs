



using System;
using UnityEngine;

namespace Karianakis.Utilities
{
    public class MyKidFatherId : MyIdBase
    {

        MyId _idKid;
        MyId _idFather;
        public MyKidFatherId(MyId father)
        {
            _idKid = new MyId();
            _idFather = father;
        }

        internal MyId EditTestGetKid => _idKid;

        /// <summary>
        /// kills ONLY THE KID the father needs to be killed seperately
        /// </summary>
        public override void KillMe()
            => _idKid.KillMe();

        internal override bool ContainsIntId(int number)
            => _idKid.ContainsIntId(number) ||
            _idFather.ContainsIntId(number);

        protected override bool OverlapsInternal(MyIdBase other)
        {
            if (other.ContainsIntId(_idKid._id))
            {
                return true;
            }
            else if (other.ContainsIntId(_idFather._id))
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
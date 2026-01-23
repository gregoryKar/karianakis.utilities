



using System;
using UnityEngine;

namespace kar_main_utils
{
  
    public class myId : myIdBase //: IEquatable<myId>, IEquatable<int>
    {


        static int idCounter;
        int _idForbidden;
        public int _id
        {
            get { return _idForbidden; }
            private set { _idForbidden = value; }
        }
        public myId()
        {
            idCounter++;
            if (idCounter > 9999) idCounter = 0;
            _id = idCounter;
        }



        public override void killAll() => invoManager.killAll(this);

    



        public override bool containsInt(int number) => _id == number;


        public override bool Equals(myIdBase other)
        {
            if (other.containsInt(_id) is false) return false;
            return true;

            //if (other is myId) return getIdType() == other.getIdType();
            // if (other is myKidFatherId dual) return dual.Equals(this);
            // else
            // {

            //     Debug.LogError("TYPE A = " + getIdType() + " B = " + other.getIdType());
            //     throw new NotImplementedException("ID EQUATION CASE");
            // }

        }

    }
}
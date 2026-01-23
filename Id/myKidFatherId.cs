



using System;
using UnityEngine;

namespace kar_main_utils
{
    //! need to seperate ids ??? like touch id and button id ?? 

    public class myKidFatherId : myIdBase
    {



        myId _idKid;
        myId _idFather;
        public myKidFatherId(myId father)
        {
            _idKid = new myId();
            _idFather = father;
        }
        public myId getKid => _idKid;
        public myId getFather => _idFather;


        public void killKid()
        {
            _idKid.killAll();
        }


        //! WILL NOT BE USED - ONLY KILL KID FOR NOW
        public void killFather_CAREFULL()
        {
            Debug.LogError("FORBIDEN FOR NOW WHY NEEDED ???");
            Debug.LogError("FORBIDEN FOR NOW WHY NEEDED ???");
            Debug.LogError("FORBIDEN FOR NOW WHY NEEDED ???");
        }
        public override void killAll()
        {
            Debug.LogError("USE killKid INSTEAD");
            Debug.LogError("USE killKid INSTEAD");
            Debug.LogError("USE killKid INSTEAD");
        }





        public override bool containsInt(int number) =>
        _idKid.containsInt(number) || _idFather.containsInt(number);

        public override bool Equals(myIdBase other) =>
        _idKid.Equals(other) || _idFather.Equals(other);



    }
}
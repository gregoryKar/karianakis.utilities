

using System.Collections.Generic;
using UnityEngine;


namespace Karianakis.Utilities
{
    class TestUnityUtilities : MonoBehaviour
    {


        private void Start()
        {

            MyId father = new MyId();
            MyKidFatherId kidFather = new MyKidFatherId(father);

            Debug.Log(father.Overlaps(null)); //??? false
            Debug.Log(father.Overlaps(kidFather)); //? true
            Debug.Log(father.Overlaps(kidFather.EditTestGetKid)); //? false
            Debug.Log(kidFather.Overlaps(father)); //? true
            Debug.Log(kidFather.Overlaps(kidFather.EditTestGetKid)); //? true





            var toEna = Invo.Simple(
                () =>
                {
                    Debug.Log("Hello World");
                }, 2f);


            var toAllo = Invo.Repeat(
           () =>
           {
               Debug.Log("Hello World");
           }, 2f, 15)
           .SetStartDelay(1f)
           .SetEndAction(() => Debug.Log("Finished!"));

            var toInfinite = Invo.Infinite(
                () =>
                {
                    Debug.Log("Hello World");
                }, 2f)
                .SetStartDelay(1f);

            var toAdvanced = InvoAdvanced.Repeat(
                (invaki) =>
                {

                    //invaki.GetIterationIndex

                    if (Time.time > 2) invaki.KillMe();
                    Debug.Log("Hello World");
                }, 2f, 15);


            bool value = toEna.CompareTo(toAllo) < 0; //? toEna is sooner than toAllo




        }


    }
}
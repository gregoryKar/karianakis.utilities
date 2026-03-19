

using System.Collections.Generic;
using UnityEngine;


namespace Karianakis.Utilities
{
    class TestUnityUtilities : MonoBehaviour
    {


        private void Start()
        {



            TestHeapValidityWithSetStartTime();
            return;



            /*
                MyId father = new MyId();
                MyKidFatherId kidFather = new MyKidFatherId(father);

                Debug.Log(father.Overlaps(null)); //??? false
                Debug.Log(father.Overlaps(kidFather)); //? true
                Debug.Log(father.Overlaps(kidFather.EditTestGetKid)); //? false
                Debug.Log(kidFather.Overlaps(father)); //? true
                Debug.Log(kidFather.Overlaps(kidFather.EditTestGetKid)); //? true
                      */



            int counter = 0;
            InvoAdvanced.Repeat(
                (invaki) =>
                {
                    int iteration = invaki.GetIterationIndex;

                    if (UnityEngine.Random.Range(0, 100) < 20)
                    {
                        Debug.LogError("i kill you");
                        invaki.KillMe();
                        return;
                    }

                    Debug.LogError($"Iteration: {iteration} counter: {counter}");

                    counter++;

                }, 1f, 15)
                .SetStartDelay(1f)
                .SetEndAction(() => Debug.Log("Finished!"))
                .SetDeathAction(() => Debug.LogError("I was killed!"));


            return;

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

        void TestHeapValidityWithSetStartTime()
        {
                //0.5- 1- 1.1- 1.2- 2
                //0.5- 1- 1.1- 1.2- 0.7
             

            Invo.Simple(() => Debug.Log($"Hello {Time.time}"), 0.5f);
            Invo.Simple(() => Debug.Log($"Hello {Time.time}"), 1f);
            Invo.Simple(() => Debug.Log($"Hello {Time.time}"), 1.1f);
            Invo.Simple(() => Debug.Log($"Hello {Time.time}"), 1.2f);
            Invo.Simple(() => Debug.Log($"Hello {Time.time}"), 2f)
                .SetStartDelay(0.7f);

        }


    }
}
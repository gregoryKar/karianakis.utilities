using System.Collections.Generic;
using UnityEngine;

namespace Karianakis.Utilities
{
    class TestUnityUtilities : MonoBehaviour
    {

        [SerializeField] float[] _delays;
        [SerializeField] DescriptiveDelays _descriptiveDelays;
        private void Start()
        {

            TestIdOverlap();

            Linvo.Global(transform)
            .AddPosition(Vector2.one)
            .SetDuration(1f)
            .SetEndAction(() => Debug.Log("Finished!"));

            InvoAdvanced.Infinite((builder) =>
            {
                Debug.Log($"Repeating {Time.time}");
            }, 1f).SetDelay
            (0.5f).SetEndAction(() => Debug.Log("Finished repeating!"))
            .SetDeathAction(() => Debug.Log("KILLED"));


            InvoGroup.Create(1f)
                .ThenDo(A)
                .ThenDo(B)
                .ThenDo(C)
                .ThenDo(D)
                .DoEveryTimeAFTER(() => Debug.Log($"AFTER EVERY ACTION {Time.time}"))
                .SetDelayArray(_descriptiveDelays.GetDelays())
                .SetEndAction(() => Debug.Log("Finished!"));

            MyId _id = new MyId();

            Invo.Repeat(()
            =>
            {
                Debug.Log($"Repeating {Time.time}");
            }
             , 0.5f, 10
             )
            .SetId(_id)
            .SetEndAction(() => Debug.Log("Finished repeating!"))
            .SetDeathAction(() => Debug.Log("KILLED"));

            void A() { }
            void B() { }
            void C() { }
            void D() { }




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

        void TestIdOverlap()
        {

            MyId noNullId = new MyId();
            Debug.LogError(noNullId == null);
            Debug.LogError(noNullId.Equals(null));

            MyId nullId = null;
            Debug.LogError(nullId == null);
            //Debug.LogError(nullId.Equals(null));

            MyId father = new MyId();
            MyKidFatherId kidFather = new MyKidFatherId(father);

            Debug.Log(father.Overlaps(null)); //??? false
            Debug.Log(father.Overlaps(kidFather)); //? true
            Debug.Log(father.Overlaps(kidFather.EditTestGetKid)); //? false
            Debug.Log(kidFather.Overlaps(father)); //? true
            Debug.Log(kidFather.Overlaps(kidFather.EditTestGetKid)); //? true

            Debug.Log(father == (null)); //??? false

            Debug.Log(father.StrictReferenceEquals(kidFather)); //? true
            Debug.Log(father == kidFather.EditTestGetKid); //? false
            Debug.Log(kidFather.Overlaps(father)); //? true
            Debug.Log(kidFather.Overlaps(kidFather.EditTestGetKid)); //? true

            MyId id0 = new MyId();
            MyId id1 = new MyId();
            if (id0 == id1) { }
            if (id0.Equals(id1)) { }



        }

    }
}
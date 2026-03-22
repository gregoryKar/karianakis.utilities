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

            InvoGroup.Create(1f)
                .ThenDo(() => Debug.Log("First"))
                .ThenDo(() => Debug.Log("Second"))
                .ThenDo(() => Debug.Log("Third"))
                .ThenDo(() => Debug.Log("Fourth"))
                .DoEveryTimeAFTER(() => Debug.Log($"AFTER EVERY ACTION {Time.time}"))
                .SetDelayArray(_descriptiveDelays.GetDelays())
                .SetEndAction(() => Debug.Log("Finished!"));





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
            MyId father = new MyId();
            MyKidFatherId kidFather = new MyKidFatherId(father);

            Debug.Log(father.Overlaps(null)); //??? false
            Debug.Log(father.Overlaps(kidFather)); //? true
            Debug.Log(father.Overlaps(kidFather.EditTestGetKid)); //? false
            Debug.Log(kidFather.Overlaps(father)); //? true
            Debug.Log(kidFather.Overlaps(kidFather.EditTestGetKid)); //? true

        }

    }
}
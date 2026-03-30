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
            TestPauseResumeStart();
        }

        void Update()
        {
            TestPauseResumeUpdate();
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



        //? TEST PAUSE RESUME SINVOKES
        MyId testPauseId;
        [Space(15)]
        [Header("TEST PAUSE RESUME")]
        [SerializeField] bool _changePauseStatus;
        [SerializeField] bool _pauseStatus;

        [SerializeField] bool _changeKillStatus;

        [SerializeField] bool _changeEndStatus;


        InvoBuilder _testPauseInvo;
        void TestPauseResumeStart()
        {
            testPauseId = new MyId();
            _testPauseInvo = Invo.Infinite(
                () =>
                {
                    Debug.LogWarning($"FOR PAUSE time = {Time.time}");
                    transform.position += Vector3.up * 0.1f;
                }
            , 0.5f);
            _testPauseInvo.SetId(testPauseId)
            .SetEndAction(() => Debug.LogError($"ENDED  time = {Time.time}"))
            .SetDeathAction(() => Debug.LogError($"DIED  time = {Time.time}"));

        }
        void TestPauseResumeUpdate()
        {
            if (_changePauseStatus)
            {
                if (_pauseStatus)
                {
                    Debug.Log("attempt to pause");
                    //InvoManager.PauseAll(testPauseId);
                    _testPauseInvo.Pause();
                }
                else
                {
                    Debug.Log("attempt to resume");
                    _testPauseInvo.Resume();
                    //InvoManager.ResumeAll(testPauseId);
                    Debug.LogError($"RESUME TIME = {Time.time} DELAY = {_testPauseInvo.GetDelay}");
                }
                _changePauseStatus = false;
            }

            if (_changeKillStatus)
            {
                if (_changeKillStatus)
                {
                    Debug.Log("attempt to kill");
                    _testPauseInvo.Kill();
                    //InvoManager.KillAll(testPauseId);
                    _changeKillStatus = false;
                }
            }

            if (_changeEndStatus)
            {
                if (_changeEndStatus)
                {
                    Debug.Log("attempt to end");
                    _testPauseInvo.End();
                    //InvoManager.EndAll(testPauseId);
                    _changeEndStatus = false;
                }
            }
        }
    }
}

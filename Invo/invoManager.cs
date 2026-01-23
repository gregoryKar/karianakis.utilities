
using System.Collections.Generic;
using UnityEngine;

namespace kar_main_utils
{


    public class invoManager : MonoBehaviour
    {

        #region  BASICS

        static invoManager instForbidden;

        static invoManager inst
        {
            get
            {
                if (instForbidden == null)
                {
                    var gameObject = new GameObject("invoManager");
                    instForbidden = gameObject.AddComponent<invoManager>();
                }
                return instForbidden;
            }

        }

        List<invoBase> invokes = new();

        #endregion


        // thought for game and normal invos habing bools 
        // and need both true .. after not ready game > gameInvos true px

        // INVO UPDATES (they run every frame) PENDING

        // posssibility of being invoked again possibly after possition change ??
        // i put check for now

        [SerializeField] int invoCount, invoProccessed, invoChecked;
        void Update()
        {
            //! FOR NOW ZERO OPTIMISATION
            //! for now if finds one not done LEAVE cause they are sorted .. ..

            invoProccessed = 0;
            invoChecked = 0;
            int i = invokes.Count - 1;

            while (invokes.Count > 0
            && i < invokes.Count
            && i > -1)
            {
                invoChecked++;
                invoBase _invo = invokes[i];

                //if (myTime.passedGlobal(_invo._end) is false) break;
                //if (Time.timeSinceLevelLoad < _invo._end) { i--; continue; }
                if (myTime.mommentPassed(_invo._end) is false) { i--; continue; }

                invoProccessed++;
                if (_invo._killMe is false)
                {
                    _invo.invokeMe(_invo);
                    _invo.process();
                }
                else invokes.RemoveAt(i);

                i--;




                //! REPEAT PROCESSING
                // if (!_invo._infinite) _invo._repeatsLeft--;
                // if (!_invo._infinite && _invo._repeatsLeft <= 0)
                // else _invo._end = myTime.now + _invo._delay;

                // else
                // {
                //     sortItem(_invo);
                //     if (myTime.passedGlobal(_invo._end))
                //     {
                //         Debug.LogError("SERIOUS PROBLEM");
                //         Debug.LogError("INVO WILL GO INFINITE");
                //         _invo._end = myTime.now + 0.1f;
                //     }
                // }


            }

            invoCount = invokes.Count;

        }



        //public static void add(invoBase thisOne) => inst.addSorted(thisOne);
        public static void add(invoBase thisOne) => inst.invokes.Add(thisOne);

        public static void killAll(myId id)
        {
            //todo duplicate same frame killAll with same id check ..

            //if (id == null) { Debug.LogError(" KILL NULL ID "); }
            foreach (var item in inst.invokes)
            {
                if (item._id == null) continue;

                if (item._id.Equals(id) is false) continue;

                // Debug.Log("KILLED ID " + item._id._id);
                item._killMe = true;
            }

        }


        // void addSorted(invoBase thisOne)
        // {
        //     if (invokes.Count == 0)
        //     {
        //         invokes.Add(thisOne);
        //         return;
        //     }

        //     int index = invokes.FindIndex(inv => thisOne._end < inv._end);
        //     if (index == -1)
        //     {
        //         invokes.Add(thisOne);
        //     }
        //     else
        //     {
        //         invokes.Insert(index, thisOne);
        //     }

        // }
        //void sortItem(invoBase thisOne)
        //{
        //    invokes.Remove(thisOne);
        //addSorted(thisOne);
        //}


        public static void remove(invoBase thisOne) => inst.invokes.Remove(thisOne);






    }

}
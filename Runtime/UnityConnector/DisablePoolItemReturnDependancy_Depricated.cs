namespace Karianakis.Utilities
{

    using UnityEngine;


    public class DisablePoolItemReturnDependancy_Depricated : MonoBehaviour
    {
        I_PoolItem _item;
        void Awake()
        {
            _item = GetComponent<I_PoolItem>();
            if (_item == null)
            {
                Debug.LogError("DisableReturnDependancy requires a component that implements I_PoolItem");
            }
        }

        bool _returning = false;
        void OnDisable()
        {

            throw new System.Exception("do not use this it is a possible root of development bur abandoned , issues -> return to pool indepedantly -> disable -> return to pool again , cant change parent or something when seting on and off ???");

            if (_returning)
            {
                Debug.LogError("apparently the guard was required DisablePoolItemReturnDependancy is trying to return an item to the pool while it's already returning, possible from the disable -> return to pool -> disable again -> retrun loop");

                return;
            }

            _returning = true;
            _item?.ReturnToPool();
            _returning = false;
        }
    }
}
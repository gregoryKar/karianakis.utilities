namespace Karianakis.Utilities
{

    using UnityEngine;


    public class DisablePoolItemReturnDependancy : MonoBehaviour
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
using UnityEngine;
namespace Karianakis.Utilities
{
    public abstract class UnityPoolMaster : PoolMaster
    {

        protected override void OnRegisterPool<T>(I_Pool pool)
        {
            if (pool is I_UnityPool unityPool)
            {
                var go = new GameObject($"Pool_{typeof(T).Name}");
                go.transform.SetParent(GetPoolsMainFather());
                unityPool.SetParent(go.transform);
            }
        }
        protected abstract Transform GetPoolsMainFather();

    }
}
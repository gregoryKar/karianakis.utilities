using UnityEngine;
namespace Karianakis.Utilities
{
    public abstract class UnityPoolMaster : PoolMaster
    {

        protected override void OnRegisterPool<T>(I_Pool pool)
        {
            if (pool is KarianakisPool<T> karianakisPool)
            {
                var go = new GameObject($"Pool_{typeof(T).Name}");
                go.transform.SetParent(GetPoolsMainFather());
                karianakisPool.SetParent(go.transform);
            }
        }
        protected abstract Transform GetPoolsMainFather();

    }
}
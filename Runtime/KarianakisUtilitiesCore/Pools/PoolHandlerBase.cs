using System.Collections.Generic;
namespace Karianakis.Utilities
{
    public abstract class PoolHandlerBase
    {
        protected List<I_Pool> _pools = new List<I_Pool>();
        public void RemoveAllActiveItems()
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                _pools[i].RemoveAllActiveItems();
            }
        }
        public void KillAllWithId(MyIdBase id)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                _pools[i].RemoveAllActiveItemsWithId(id);
            }
        }
    }
}
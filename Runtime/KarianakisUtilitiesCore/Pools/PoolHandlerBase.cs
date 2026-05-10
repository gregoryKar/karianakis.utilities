using System;
using System.Collections.Generic;
namespace Karianakis.Utilities
{
    public abstract class PoolHandlerBase
    {
        List<I_Pool> _pools = new List<I_Pool>();
        protected void RegisterPool<T>(I_Pool pool)
        {
            _pools.Add(pool);
            _poolsDictionary[typeof(T)] = pool;   
        }
       


        Dictionary<Type, I_Pool> _poolsDictionary = new();
        public T Get<T>(string theName, MyIdBase id)
        {
            var type = typeof(T);
            if (_poolsDictionary.ContainsKey(type))
            {
                return ((KarianakisPool<T>)_poolsDictionary[type]).Get(theName, id);
            }
            else
            {
                EngineConnector.Error("No pool of type " + type + " found");
            }
            return default;
        }

        public void Remove<T>(T item) 
        {
            var type = typeof(T);
            if (_poolsDictionary.ContainsKey(type))
            {
                ((KarianakisPool<T>)_poolsDictionary[type]).Remove(item);
            }
            else
            {
                EngineConnector.Error("No pool of type " + type + " found");
            }
        }




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
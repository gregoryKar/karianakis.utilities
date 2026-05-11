using System;
using System.Collections.Generic;
namespace Karianakis.Utilities
{
    public class PoolMaster : I_PoolMaster
    {
        List<I_Pool> _pools = new List<I_Pool>();
        public void RegisterPool<T>(I_Pool pool)
        {
            if (_poolsDictionary.ContainsKey(typeof(T)))
            {
                EngineConnector.Error("Pool of type " + typeof(T) + " already registered");
                return;
            }
            else
            {
                _pools.Add(pool);
                _poolsDictionary[typeof(T)] = pool;
            }
        }

        Dictionary<Type, I_Pool> _poolsDictionary = new();
        public T GetSigned<T>(string theName, MyIdBase id)
        {
            var type = typeof(T);
            if (_poolsDictionary.ContainsKey(type))
            {
                return ((I_SpecificPool<T>)_poolsDictionary[type]).GetSigned(theName, id);
            }
            else
            {
                EngineConnector.Error("No pool of type " + type + " found");
            }
            return default;
        }

        public T Get<T>()
        {
            var type = typeof(T);
            if (_poolsDictionary.ContainsKey(type))
            {
                return ((I_SpecificPool<T>)_poolsDictionary[type]).Get();
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
                ((I_SpecificPool<T>)_poolsDictionary[type]).Remove(item);
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
        public void RemoveAllActiveItemsWithId(MyIdBase id)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                _pools[i].RemoveAllActiveItemsWithId(id);
            }
        }


    }
}
using System;
using System.Collections.Generic;
using System.Linq;
namespace Karianakis.Utilities
{
    public class KarianakisPool<T> : I_Pool
    {

        public KarianakisPool(object parent, string preffix)
        {
            _preffix = preffix;
        }

        protected string _preffix;



        HashSet<T> _active { get; set; } = new();
        Stack<T> _inactive { get; set; } = new();

        Func<T> _instantiate;
        Action<T> _initialize;
        Action<T> _deactivate;

        public void SetInstantiate(Func<T> instantiate)
            => _instantiate = instantiate;
        public void SetInitialize(Action<T> initialize)
            => _initialize = initialize;
        public void SetDeactivate(Action<T> deactivate)
            => _deactivate = deactivate;



        internal T GetRaw()
        {
            T node;
            int size = _inactive.Count;
            if (size > 0)
            {
                node = _inactive.Pop();
                OnInitialise(node);
            }
            else
            {
                node = _instantiate();
                OnIntantiate(node);
            }
            _initialize(node);
            _active.Add(node);
            return node;
        }
        internal T Get(string theName, MyIdBase id)
        {
            T node = GetRaw();
            AssignName(node, theName);
            if(node is I_HaveIdExtended signable)
            {
                signable.SetId(id);
            }
            return node;
        }




        public void Remove(T node)
        {

            if (_active.Contains(node))
            {
                OnDeactivate(node);

                _active.Remove(node);
                _deactivate(node);
                _inactive.Push(node);
            }
            else //! ERROR CASE
            {
                Type type = typeof(T);

                EngineConnector.Error($"KarianakisPool : Attempted to remove a node that is not active. Type: {type} ");
            }

        }
        public void RemoveAllActiveItems()
        {
            var array = _active.ToArray();
            for (int i = 0; i < array.Length; i++)
            {
                Remove(array[i]);
            }
        }
        public T[] GetAllActiveItems() => _active.ToArray();




        protected virtual void OnIntantiate(T item) { }
        protected virtual void OnInitialise(T item) { }
        protected virtual void AssignName(T item, string givenName) { }
        protected virtual void OnDeactivate(T item) { }

        public void RemoveAllActiveItemsWithId(MyIdBase id)
        {
            //typeof(T) is not I_HaveId
            if (typeof(I_HaveId).IsAssignableFrom(typeof(T)) == false)
            {
                return;
            }

            if (id == null)
            {
                EngineConnector.Error($"KarianakisPool : Attempted to remove items with id, but given id was null. Type: {typeof(T)}");
                return;
            }


            var array = _active.ToArray();
            for (int i = 0; i < array.Length; i++)
            {

                T item = array[i];
                if (item is I_HaveId hasId)
                {
                    if (hasId.GetId() != null)
                    {

                        if (hasId.GetId().Equals(id))
                        {
                            Remove(item);
                        }
                    }
                }


            }
        }


    }
}



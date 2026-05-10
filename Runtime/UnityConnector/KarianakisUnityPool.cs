using UnityEngine;
namespace Karianakis.Utilities
{
    public class KarianakisUnityPool<T> : KarianakisPool<T> where T : Component
    {
        public KarianakisUnityPool(Transform parent, string preffix) : base(parent, preffix) { _parent = parent; }

        Transform _parent;

      
        
        protected override void OnIntantiate(T item)
        {
            if (_parent != null)
            {
                item.transform.SetParent(_parent);
            }
            item.name = _preffix + "_NEW";
        }

        protected override void AssignName(T item, string givenName)
        {
            item.name = $"{_preffix}_{givenName}";
        }

        protected override void OnDeactivate(T item)
        {
            if (_parent != null)
            {
                item.transform.SetParent(_parent);
            }
            item.name += "_OFF";
        }

    }


}
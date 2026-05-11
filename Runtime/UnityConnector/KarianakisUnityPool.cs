using UnityEngine;
namespace Karianakis.Utilities
{
    public class KarianakisUnityPool<T> : KarianakisPool<T> where T : Component
    {

        Transform _parent;
        public override void SetParent(object parent)
        {
            _parent = parent as Transform;
        }
      
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
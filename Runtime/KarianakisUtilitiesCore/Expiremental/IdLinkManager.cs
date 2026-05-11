using System;
using System.Collections.Generic;
namespace Karianakis.Utilities
{
    public class IdLinkManager
    {

        static IdLinkManager _instance;
        internal IdLinkManager()
        {
            _instance = this;
        }
       

        Dictionary<MyIdBase, List<I_IdLinkedItem>> _links = new();
        void LinkLocal(MyIdBase id, I_IdLinkedItem item)
        {
            id.NotifyHaveLinkedItems();
            if (_links.TryGetValue(id, out var list) == false)
            {
                list = new List<I_IdLinkedItem>();
                _links[id] = list;
            }
            list.Add(item);
        }
        void RemoveAllLinkedItemsLocal(MyIdBase id)
        {
            id.NotifyClearedLinkedItems();
            if (_links.TryGetValue(id, out var list))
            {
                foreach (var item in list)
                {
                    item.RemoveMe();
                }

                _links.Remove(id);
            }
        }

        void ClearAllLocal()
        {
            foreach (var pair in _links)
            {
                foreach (var item in pair.Value)
                {
                    item.RemoveMe();
                }
                pair.Key.NotifyClearedLinkedItems();
            }
            _links.Clear();
        }


        //? EXPOSED
        public static void Link(MyIdBase id, I_IdLinkedItem item)
        {
            _instance.LinkLocal(id, item);
        }
        public static void RemoveAllLinkedItems(MyIdBase id)
        {
            _instance.RemoveAllLinkedItemsLocal(id);
        }
        public static void ClearAll()
        {
            _instance.ClearAllLocal();
        }

    }
}
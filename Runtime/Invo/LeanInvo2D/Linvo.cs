

using UnityEngine;

namespace Karianakis.Utilities
{
    public class Linvo
    {
        public static LeanInvo2DBuilder Global(Transform transform)
        {
            return new LeanInvo2DBuilder(transform , false);
        }
        public static LeanInvo2DBuilder Local(Transform transform)
        {
            return new LeanInvo2DBuilder(transform , true);
        }
    }
}
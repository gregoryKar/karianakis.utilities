namespace Karianakis.Utilities
{
    public interface I_PoolMaster : I_Pool
    {
        public T Get<T>();
        public T GetSigned<T>(string theName, MyIdBase id);
        public void Remove<T>(T item);
        public void RegisterPool<T>(I_Pool pool);

    }

}
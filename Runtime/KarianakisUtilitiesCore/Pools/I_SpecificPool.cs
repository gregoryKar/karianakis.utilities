namespace Karianakis.Utilities
{
    public interface I_SpecificPool<T> : I_Pool
    {
        public T GetSigned(string theName, MyIdBase id);
        public T Get();
        public void Remove(T item);
    }

}
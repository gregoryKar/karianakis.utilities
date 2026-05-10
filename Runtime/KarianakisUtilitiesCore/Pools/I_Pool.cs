namespace Karianakis.Utilities
{
    public interface I_Pool
    {
        public void RemoveAllActiveItems();
        public void RemoveAllActiveItemsWithId(MyIdBase id);
    }

}
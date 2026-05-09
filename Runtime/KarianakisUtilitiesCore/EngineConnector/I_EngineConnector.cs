using System;
namespace Karianakis.Utilities
{
    public interface I_EngineConnector

    {
        //? DEBUG
        public void Log(Object say);
        public void Error(Object say);
        public void Warning(Object say);
        public void Break();

        //? TIME
        public float GetTimeNow { get; }


        //? ENGINE
        public bool GetIsInEditor { get; }
        public bool GetIsKarianakis()
        {
#if KARIANAKIS
                return true;
#endif
            return false;
        }

    }
}
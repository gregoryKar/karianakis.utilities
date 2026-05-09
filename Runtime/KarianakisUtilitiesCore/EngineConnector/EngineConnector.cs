using System;

namespace Karianakis.Utilities
{
    class EngineConnector
    {
        static I_EngineConnector _engineConnector;

        public EngineConnector(I_EngineConnector engineConnector)
        {
            _engineConnector = engineConnector;
        }



        //? TIME
        public static float GetTimeNow()
        {
            if (_engineConnector == null)
            {
                return -69f;
            }
            else
            {
                return _engineConnector.GetTimeNow;
            }
        }
       


        //? DEBBUG
        public static void Log(Object print)
        {
            if (_engineConnector != null)
            {
                _engineConnector.Log(print);
            }
        }
        public static void Error(Object print)
        {
            if (_engineConnector != null)
            {
                _engineConnector.Error(print);
            }
        }
        public static void Warning(Object print)
        {
            if (_engineConnector != null)
            {
                _engineConnector.Warning(print);
            }
        }
        public static void Break()
        {
            if (_engineConnector != null)
            {
                _engineConnector.Break();
            }
        }


        //? ENGINE
        public static bool GetIsInEditor()
        {
            if (_engineConnector != null)
            {
                return _engineConnector.GetIsInEditor;
            }
            else
            {
                return false;
            }
        }
    

        public static bool GetIsKarianakis()
        {
            if (_engineConnector != null)
            {
                return _engineConnector.GetIsKarianakis();
            }
            else
            {
                return false;
            }
        }


    }
}
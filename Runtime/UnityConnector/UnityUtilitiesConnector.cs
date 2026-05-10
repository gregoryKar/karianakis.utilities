using UnityEngine;
namespace Karianakis.Utilities
{
    public class UnityUtilitiesConnector : MonoBehaviour, I_EngineConnector
    {

        InvoManager _manager;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnStaticInitialize()
        {
            var go = new GameObject("UnityUtilitiesConnector");
            DontDestroyOnLoad(go);
            var connector = go.AddComponent<UnityUtilitiesConnector>();
            new EngineConnector(connector);
            connector._manager = new InvoManager();
            new IdLinkManager();//! NEW
        }


        public void FixedUpdate()
        {
            _manager.UpdateMe();

            _invokeCount = _manager.GetInvoCount();
            _invokeEnd = _manager.GetInvokesEndTimes();

            //Log($"t{GetTimeNow} i{_invokeCount} dl{_invokeEnd.Length}");
        }

        [SerializeField] int _invokeCount = -1;
        [SerializeField] float[] _invokeEnd;


        public float GetTimeNow => Time.timeSinceLevelLoad;


        public bool GetIsInEditor => Application.isEditor;


        public void Break()
        {
            Debug.Break();
        }

        public void Error(object say)
        {
            Debug.LogError(say);
        }

        public void Log(object say)
        {
            Debug.Log(say);
        }

        public void Warning(object say)
        {
            Debug.LogWarning(say);
        }


    }
}
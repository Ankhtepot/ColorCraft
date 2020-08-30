using UnityEngine;
using UnityEngine.Events;

namespace HelperClasses
{
    public static class CustomUnityEvents
    {
        [System.Serializable]
        public class EventVector3Int : UnityEvent<Vector3Int> { }
        
        [System.Serializable]
        public class EventBool : UnityEvent<bool> { }
    }
}
using UnityEngine;
using UnityEngine.Events;

namespace HelperClasses
{
    public static class CustomUnityEvents
    {
        [System.Serializable]
        public class EventVector3Int : UnityEvent<Vector3Int> { }
        
        [System.Serializable]
        public class EventVector2Int : UnityEvent<Vector2Int> { }
        
        [System.Serializable]
        public class EventBool : UnityEvent<bool> { }
        
        [System.Serializable]
        public class EventGameMode : UnityEvent<GameMode> { }
        
        [System.Serializable]
        public class EventBuildElement : UnityEvent<BuildElement> { }
    }
}
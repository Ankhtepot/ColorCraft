using Models;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Utilities
{
    public static class CustomUnityEvents
    {
        [System.Serializable]
        public class EventVector3Int : UnityEvent<Vector3Int> { }
        
        [System.Serializable]
        public class EventBool : UnityEvent<bool> { }
        
        [System.Serializable]
        public class EventSavedPosition : UnityEvent<SavedPosition> { }
        
        [System.Serializable]
        public class EventGameMode : UnityEvent<GameMode> { }
        
        [System.Serializable]
        public class EventBuildElement : UnityEvent<BuildElement> { }
        
        [System.Serializable]
        public class EventVector3IntVector3Int : UnityEvent<Vector3Int, Vector3Int> { }
    }
}
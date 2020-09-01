using System;
using System.IO;
using Models;
using UnityEngine;

namespace Utilities
{
    public static class JsonService
    {
        public static string SerializeToJson<T>(T source)
        {
            return JsonUtility.ToJson(source);
        }

        public static SavedPosition DeserializeToString(string fullPath)
        {
            return JsonUtility.FromJson<SavedPosition>(fullPath);
        }
    }
}
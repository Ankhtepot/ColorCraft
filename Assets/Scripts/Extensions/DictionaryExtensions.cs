using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class DictionaryExtensions
    {
        public static List<K> GetKeys<K, V>(this Dictionary<K, V> dictionary)
        {
            return dictionary.Select(item => item.Key).ToList();
        } 
    }
}
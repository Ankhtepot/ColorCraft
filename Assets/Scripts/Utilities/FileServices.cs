using System;
using System.IO;
using Extensions;
using Models;

namespace Utilities
{
    public static class FileServices
    {
        public static bool SavePosition(SavedPosition position, string fullPath)
        {
            if (position == null) return false;

            var json = JsonService.SerializeToJson(position);

            return SaveFile(fullPath, json);
        }

        public static SavedPosition LoadPosition(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                return JsonService.DeserializeToString(LoadJson(fullPath));
            }

            return null;
        }

        private static bool SaveFile(string fullPath, string content)
        {
            fullPath = fullPath.SanitizeFilePath();
            
            try
            {
                string saveDirectory = Path.GetDirectoryName(fullPath);
                
                if (saveDirectory != null && !Directory.Exists(saveDirectory))
                {
                    Directory.CreateDirectory(saveDirectory);
                }

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
                
                File.WriteAllText(fullPath, content);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private static string LoadJson(string fullPath)
        {
            try
            {
                return File.ReadAllText(fullPath);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
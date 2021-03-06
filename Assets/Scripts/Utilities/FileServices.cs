﻿using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            return File.Exists(fullPath) 
                ? JsonService.DeserializeToString(LoadJson(fullPath)) 
                : null;
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

        public static string ExtractPositionTitleFromScreenshotPath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return null;
            
            var extension = Path.GetExtension(filePath);
            var fileName = Path.GetFileName(filePath).Replace($"{Strings.Screenshot}{extension}", "");
            var words = Regex.Matches(fileName, @"([A-Z][a-z]+)")
                    .Cast<Match>()
                    .Select(m => m.Value);

            return string.Join(" ", words);
        }
    }
}
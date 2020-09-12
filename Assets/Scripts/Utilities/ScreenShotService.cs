using System;
using System.IO;
using UnityEngine;

namespace Utilities
{
    public class ScreenShotService : MonoBehaviour
    {
#pragma warning disable 649
        [SerializeField] private int captureWidth = 480;
        [SerializeField] private int captureHeight = 270;
        [SerializeField] private Camera targetCamera;
#pragma warning restore 649

        public string CaptureScreenshotFile(string positionFileName)
        {
            try
            {
                var bytes = GetCurrentScreenshotBytes();
                return WriteScreenshotBytesToPng(positionFileName, bytes);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public static string WriteScreenshotBytesToPng(string filename, byte[] bytes)
        {
            
            File.WriteAllBytes(GetScreenshotFileName(filename), bytes);

            return filename;
        }

        public byte[] GetCurrentScreenshotBytes()
        {
            var renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            var screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
         
            // get main camera and manually render scene into it
            targetCamera.targetTexture = renderTexture;
            targetCamera.Render();
            RenderTexture.active = renderTexture;
            screenShot.ReadPixels(new Rect(0, 0, captureWidth, captureHeight), 0, 0);

            targetCamera.targetTexture = null;
            RenderTexture.active = null;
            Destroy(renderTexture);
            
            return screenShot.EncodeToPNG();
        } 

        private static string GetScreenshotFileName(string positionFileName)
        {
            return positionFileName.Replace(".json", "Screenshot.png");
        }
        
        public Texture2D LoadScreenshot(string filePath) 
        {
            if (!File.Exists(filePath)) return null;
 
            var fileData = File.ReadAllBytes(filePath);

            return GetScreenshotFromBytes(fileData);
        }

        public Texture2D GetScreenshotFromBytes(byte[] bytes)
        {
            var texture = new Texture2D(captureWidth, captureHeight);
            texture.LoadImage(bytes); //..this will auto-resize the texture dimensions.

            return texture;
        }
    }
}
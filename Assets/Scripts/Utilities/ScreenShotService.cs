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

        public string TakeScreenShot(string positionScreenShotFilename)
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
            var bytes = screenShot.EncodeToPNG();
            var filename = GetScreenshotFileName(positionScreenShotFilename);
            File.WriteAllBytes(filename, bytes);

            return filename;
        }

        private static string GetScreenshotFileName(string positionFileName)
        {
            return positionFileName.Replace(".json", "Screenshot.png");
        }
    }
}
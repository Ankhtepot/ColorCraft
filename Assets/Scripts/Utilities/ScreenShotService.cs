using System;
using System.IO;
using UnityEngine;

namespace Utilities
{
    public class ScreenShotService : MonoBehaviour
    {
        [SerializeField] private int captureWidth = 480;
        [SerializeField] private int captureHeight = 270;
        [SerializeField] private Camera targetCamera;
        
        private Rect rect;
        private RenderTexture renderTexture;
        private Texture2D screenShot;

        private RenderTexture TakeScreenShot()
        {
            if (renderTexture == null)
            {
                // creates off-screen render texture that can rendered into
                rect = new Rect(0, 0, captureWidth, captureHeight);
                renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
                screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
            }
         
            // get main camera and manually render scene into it
            targetCamera.targetTexture = renderTexture;
            targetCamera.Render();

            return renderTexture;
        }
    }
}
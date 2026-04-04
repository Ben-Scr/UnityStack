using System.IO;
using UnityEditor;
using UnityEngine;

namespace BenScr.UnityStack
{
    /*
     * Used for capturing and saving images of a camera's view to a PNG file
     */

    public class ImageCreator : MonoBehaviour
    {
        [SerializeField] private Camera cameraToCapture;
        [SerializeField] private int textureWidth = 1920;
        [SerializeField] private int textureHeight = 1080;
        [SerializeField] private string saveFileName = "SavedImage";
        [SerializeField] private bool saveOnAwake = true;
        [SerializeField] private bool saveOnUpdate = true;
        [SerializeField] private bool enableLogs = true;

        private RenderTexture renderTexture;

        private string fullPath => Path.Combine(Application.dataPath, saveFileName + ".png");

        private void Awake()
        {
            saveFileName = saveFileName == "" ? System.Guid.NewGuid().ToString() : saveFileName;
            renderTexture = new RenderTexture(textureWidth, textureHeight, 0, RenderTextureFormat.ARGB32);
            cameraToCapture.targetTexture = renderTexture;
            cameraToCapture.Render();

            if (saveOnAwake)
            {
                CaptureFrame();
            }
        }

        private void Update()
        {
            if (saveOnUpdate)
            {
                CaptureFrame();
                saveOnUpdate = false;
            }
        }

        public void CaptureFrame()
        {
            RenderTexture.active = renderTexture;
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            RenderTexture.active = null;
            SaveTextureAsPNG(texture);
            Destroy(texture);
        }

        private void SaveTextureAsPNG(Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            File.WriteAllBytes(fullPath, bytes);

            if (enableLogs)
                Debug.Log($"Saved image to: {fullPath}");
        }
    }
}



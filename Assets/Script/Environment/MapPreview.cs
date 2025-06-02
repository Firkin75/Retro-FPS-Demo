using UnityEngine;
using System.IO;

public class MapPreviewCapture : MonoBehaviour
{
    public Camera mapCamera;
    public RenderTexture renderTexture;

    public void CaptureMapPreview()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        mapCamera.Render();

        Texture2D image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();

        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/MapPreview.png", bytes);

        RenderTexture.active = currentRT;

        Debug.Log(  Application.dataPath + "/MapPreview.png");
    }
}

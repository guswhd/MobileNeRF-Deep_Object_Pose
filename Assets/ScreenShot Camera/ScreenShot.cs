using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CustomUtils.ScreenShot))]
public class ScreenShotEditor : Editor 
{
    CustomUtils.ScreenShot screenShot;
	void OnEnable() => screenShot = target as CustomUtils.ScreenShot;

	public override void OnInspectorGUI()
	{

        base.OnInspectorGUI();
        if (GUILayout.Button("ScreenShot"))
        {

            screenShot.ScreenShotClick();
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        } 
	}
}
#endif

namespace CustomUtils
{
    public class ScreenShot : MonoBehaviour
    {
        MakeJson makeJson;
        public int Png_Amount = 0;
        int testNumber = 0;
        private void Awake()
        {
            makeJson = GetComponent<MakeJson>();
        }
        
        public void ScreenShotClick()
        {
            StartCoroutine(CaptureScreenshots());
        }
        
        IEnumerator CaptureScreenshots()
        {
            for (int i = 0; i < Png_Amount; i++)
            {
                yield return StartCoroutine(CoroutineScreenShot(i));
            }
        }

        IEnumerator CoroutineScreenShot(int i)
        {
            
            makeJson.JsonSave((i), "c:\\Users\\user\\Desktop\\cpastone");

            RenderTexture renderTexture = MakeJson.JsonCam.targetTexture;
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);

            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            File.WriteAllBytes($"{"c:\\Users\\user\\Desktop\\cpastone"}/{(i).ToString()}.png", texture.EncodeToPNG());

            makeJson.RandomMoveObjects();

            yield return new WaitForSeconds(1f);
        }
    }
}
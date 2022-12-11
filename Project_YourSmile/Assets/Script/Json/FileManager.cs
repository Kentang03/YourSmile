using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class FileManager : MonoBehaviour
{
   string path ="D:/Project_YourSmile/Project_YourSmile/Assets/Asset Image/UI/Cahya/Asset_Potrait_Cahya_Marah.png";
   //string path;
   public RawImage rawImage;


    void Start()
    {
        StartCoroutine(GetTexture());
    }
   public void OpenFileExplorer()
   {

    path = EditorUtility.OpenFilePanel("Show all Image (.png)", "", "png");
    StartCoroutine(GetTexture());

   }

   IEnumerator GetTexture()
   {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path);
        Debug.Log(path);

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            
        }
        else
        {
            Debug.Log("Load Successed");
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage.texture = myTexture;
            
        }
   }
    
}

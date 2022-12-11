using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class JsonCombIne : MonoBehaviour
{
    public TextAsset textJSON;
    
    //public RawImage rawImage1;
    //public RawImage rawImage2;
    

    [System.Serializable]

    public class Player
    {
        public string leftName;
        public string rightName;
        public string Dialogue;
        public string path1;
        public string path2;
        //public Image rightImage;
        //public Image leftImage;

    }

    [System.Serializable]

    public class PlayerList
    {
        public Player[] player;
    }

    public PlayerList myPlayerList = new PlayerList();

    void Start()
    {
        myPlayerList = JsonUtility.FromJson<PlayerList>(textJSON.text);
        //StartCoroutine(GetTexture1());
        //StartCoroutine(GetTexture2());
     
    }

   /* IEnumerator GetTexture1()
   {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path1);
        

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage1.texture = myTexture;
            
        }
   }

     IEnumerator GetTexture2()
   {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture("file:///" + path2);
      

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            rawImage2.texture = myTexture;
            
        }
   }*/

}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class JSONreader : MonoBehaviour
{
    public TextAsset textJSON;

    [System.Serializable]

    public class Player
    {
        //public string url = "https://upload.wikimedia.org/wikipedia/commons/thumb/b/b8/2021_Facebook_icon.svg/1024px-2021_Facebook_icon.svg.png";
        public Renderer thisRenderer;
        public string leftName;
        public string rightName;
        public string Dialogue;
        public Image leftImage;
        public Image rightImage;

    }

    [System.Serializable]

    public class PlayerList
    {
        public Player[] player;
    }

    public PlayerList myPlayerList = new PlayerList();
    // Start is called before the first frame update
    void Start()
    {
        myPlayerList = JsonUtility.FromJson<PlayerList>(textJSON.text);
       // StartCoroutine(LoadFromLikeCoroutine());
        //thisRenderer.material.color = Color.red;
    }

    /*private IEnumerator LoadFromLikeCoroutine()
    {
        Debug.Log("Loading.....");
        //WWW wwwLoader = new WWW(url);   
        //yield return wwwLoader;

        Debug.Log("Loaded");
        thisRenderer.material.color = Color.white;
        thisRenderer.material.mainTexture = wwwLoader.texture;
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene3 : MonoBehaviour
{
    void OnEnable() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("InGameDialogSystem4" , LoadSceneMode.Single);
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene5 : MonoBehaviour
{
    void OnEnable() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene("InGameDialogSystem7" , LoadSceneMode.Single);
    }

    
}

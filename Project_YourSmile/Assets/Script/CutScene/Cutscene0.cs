using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene0 : MonoBehaviour
{
    void OnEnable() 
    {
        SceneManager.LoadScene("InGameCutScene1" , LoadSceneMode.Single);
    }

    
}

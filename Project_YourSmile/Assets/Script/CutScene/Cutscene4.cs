using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene4 : MonoBehaviour
{
    void OnEnable() 
    {
        SceneManager.LoadScene("InGameDialogSystem5" , LoadSceneMode.Single);
    }

    
}

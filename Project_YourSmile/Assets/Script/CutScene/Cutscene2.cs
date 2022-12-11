using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene2 : MonoBehaviour
{
    void OnEnable() 
    {
        SceneManager.LoadScene("InGameDialogSystem2" , LoadSceneMode.Single);
    }

    
}

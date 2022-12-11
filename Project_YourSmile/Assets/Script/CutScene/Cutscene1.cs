using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene1 : MonoBehaviour
{
    void OnEnable() 
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}

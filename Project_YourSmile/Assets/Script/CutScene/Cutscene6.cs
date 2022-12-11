using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cutscene6 : MonoBehaviour
{
    void OnEnable() 
    {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    
}

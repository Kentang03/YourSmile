using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PopUpDialogSystem: MonoBehaviour
{
   public GameObject gameOverMenu;

    private void OnEnable() 
    {
        EnemyProp.OnTargetDeath += EnableDialogSystem;

    }

     private void OnDisable() 
    {
        EnemyProp.OnTargetDeath -= EnableDialogSystem;
        
    }
    

   public void EnableDialogSystem()
   {
       gameOverMenu.SetActive(true);
   }
}

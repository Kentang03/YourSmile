using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpActiveCutsceneIn : MonoBehaviour
{
   //public GameObject DialogSystemActive;
   public GameObject DialogSystemActive2;
   public GameObject DialogSystemActive3;
   

    private void OnEnable() 
    {
        //DialogueBoxCutsceneIn.OnActive += EnableDialogBoxOuter3;
        DialogueBoxCutsceneIn.OnActive += EnableDialogBoxOuter2;
        DialogueBoxCutsceneIn.OnActive += EnableDialogBoxOuter4;

       // DialogueBoxCutsceneIn.OnActive2 += Enable2DialogBoxOuter3;
        DialogueBoxCutsceneIn.OnActive2 += Enable2DialogBoxOuter2;
        DialogueBoxCutsceneIn.OnActive2 += Enable2DialogBoxOuter4;

    }

     private void OnDisable() 
    {
        //DialogueBoxCutsceneIn.OnActive  -= EnableDialogBoxOuter3;
        DialogueBoxCutsceneIn.OnActive  -= EnableDialogBoxOuter2;
        DialogueBoxCutsceneIn.OnActive  -= EnableDialogBoxOuter4;

        //DialogueBoxCutsceneIn.OnActive2  -= Enable2DialogBoxOuter3;
        DialogueBoxCutsceneIn.OnActive2  -= Enable2DialogBoxOuter2;
        DialogueBoxCutsceneIn.OnActive2  -= Enable2DialogBoxOuter4;
        
    }
    

   ///public void EnableDialogBoxOuter3()
   //{
    //   DialogSystemActive.SetActive(true);
       
   //}

   public void EnableDialogBoxOuter2()
   {
       DialogSystemActive2.SetActive(true);
   }

   public void EnableDialogBoxOuter4()
   {
       DialogSystemActive3.SetActive(false);
   }


   // public void Enable2DialogBoxOuter3()
   //{
    //   DialogSystemActive.SetActive(false);
       
   //}

   public void Enable2DialogBoxOuter2()
   {
       DialogSystemActive2.SetActive(false);
   }

   public void Enable2DialogBoxOuter4()
   {
       DialogSystemActive3.SetActive(true);
   }
}

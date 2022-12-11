using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpActiveDialogSystem5 : MonoBehaviour
{
   //public GameObject DialogSystemActive;
   public GameObject DialogSystemActive2;
   public GameObject DialogSystemActive3;
   

    private void OnEnable() 
    {
        //DialogueBoxCutscene5.OnActive += EnableDialogBoxOuter3;
        DialogueBoxCutscene5.OnActive += EnableDialogBoxOuter2;
        DialogueBoxCutscene5.OnActive += EnableDialogBoxOuter4;

       // DialogueBoxCutscene5.OnActive2 += Enable2DialogBoxOuter3;
        DialogueBoxCutscene5.OnActive2 += Enable2DialogBoxOuter2;
        DialogueBoxCutscene5.OnActive2 += Enable2DialogBoxOuter4;

    }

     private void OnDisable() 
    {
        //DialogueBoxCutscene5.OnActive  -= EnableDialogBoxOuter3;
        DialogueBoxCutscene5.OnActive  -= EnableDialogBoxOuter2;
        DialogueBoxCutscene5.OnActive  -= EnableDialogBoxOuter4;

        //DialogueBoxCutscene5.OnActive2  -= Enable2DialogBoxOuter3;
        DialogueBoxCutscene5.OnActive2  -= Enable2DialogBoxOuter2;
        DialogueBoxCutscene5.OnActive2  -= Enable2DialogBoxOuter4;
        
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

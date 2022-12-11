using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpActiveDialogSystem8 : MonoBehaviour
{
   //public GameObject DialogSystemActive;
   public GameObject Yanto2;
   public GameObject Cahya2;
   

    private void OnEnable() 
    {
        //NewDialogueBoxTest.OnActive += EnableDialogBoxOuter3;
        NewDialogueBoxTest.OnActive += EnableYanto2;
        NewDialogueBoxTest.OnActive += EnableCahya2;

       // NewDialogueBoxTest.OnActive2 += Enable2DialogBoxOuter3;
        NewDialogueBoxTest.OnActive2 += Enable2Yanto2;
        NewDialogueBoxTest.OnActive2 += Enable2Cahya2;

    }

     private void OnDisable() 
    {
        //NewDialogueBoxTest.OnActive  -= EnableDialogBoxOuter3;
        NewDialogueBoxTest.OnActive  -= EnableYanto2;
        NewDialogueBoxTest.OnActive  -= EnableCahya2;

        //NewDialogueBoxTest.OnActive2  -= Enable2DialogBoxOuter3;
        NewDialogueBoxTest.OnActive2  -= Enable2Yanto2;
        NewDialogueBoxTest.OnActive2  -= Enable2Cahya2;
        
    }
    

   ///public void EnableDialogBoxOuter3()
   //{
    //   DialogSystemActive.SetActive(true);
       
   //}

   public void EnableYanto2()
   {
       Yanto2.SetActive(true);
   }

   public void EnableCahya2()
   {
       Cahya2.SetActive(false);
   }


   // public void Enable2DialogBoxOuter3()
   //{
    //   DialogSystemActive.SetActive(false);
       
   //}

   public void Enable2Yanto2()
   {
       Yanto2.SetActive(false);
   }

   public void Enable2Cahya2()
   {
       Cahya2.SetActive(true);
   }
}

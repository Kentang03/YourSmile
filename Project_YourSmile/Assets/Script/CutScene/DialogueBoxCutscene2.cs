using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class DialogueBoxCutscene2 : MonoBehaviour
{

    public DialogueSegment[] DialogueSegments;
    [Space]
    public Image SpeakerFaceDisplay1;
    public Image SpeakerFaceDisplay2;
    public Image DialogueBoxBorder;
    //public Image DialogueBoxInner;
    public Image SkipIndicator;
    public Image Cahya;
    public Image Yanto;
    public Animator DialogueAnimator;
    private bool StartDialogue = true;
    
    
    [Space]
    public TextMeshProUGUI SpeakerNameDisplay;
    public TextMeshProUGUI DialogueDisplay;
    [Space]

    public float TextSpeed;
    private bool CanContinue;
    private int DialogueIndex = 0;
    public float alphaLevel = .5f ;
    private SpriteRenderer sprite;

    public static event Action OnActive;
    public static event Action OnActive2;

    public TextAsset textJSON;
    //public int trigger = 0;


//public int sortingOrder = 0;
[System.Serializable]
public class DialogueSegment
{
    public string Dialogue;
    public Subject Speaker1; 
    public Subject Speaker2; 

}
[System.Serializable]
public class DialogueSegmentlist
{
    public DialogueSegment[] dialog;
   
}

public DialogueSegmentlist myDialogueSegmentList = new DialogueSegmentlist();

    void Start()
    {
        SetStyle(DialogueSegments[0].Speaker1);
        SetStyle2(DialogueSegments[0].Speaker2);
        StartCoroutine(PlayDialogue(DialogueSegments[0].Dialogue));
        sprite = GetComponent<SpriteRenderer>();


        myDialogueSegmentList = JsonUtility.FromJson<DialogueSegmentlist>(textJSON.text);

        

        //sprite.sortingOrder = sortingOrder;
    }

    void Update()
    {
        
         
            if (StartDialogue)
                {
                    DialogueAnimator.SetTrigger("Enter");
                    StartDialogue = false;
                }
                else
                {
                    nextSentence();

                }
            

    }
    void nextSentence()
    {
         SkipIndicator.enabled = CanContinue;
        if(Input.GetKeyDown(KeyCode.Tab) && CanContinue)
        {
            DialogueIndex++;
            if(DialogueIndex == DialogueSegments.Length)
            {
                //gameObject.SetActive(false);
                StartDialogue = true;
                DialogueAnimator.SetTrigger("Exit");
                SceneManager.LoadScene("InGameCutScene3" , LoadSceneMode.Single);

                DialogueIndex = 0;
                
                return;
            }

            SetStyle(DialogueSegments[DialogueIndex].Speaker1);
            SetStyle2(DialogueSegments[DialogueIndex].Speaker2);
            StartCoroutine(PlayDialogue(DialogueSegments[DialogueIndex].Dialogue));
        }
    }

    /* private void OnEnable() 
    {
        MessagePop.triggerDialog += EnableDialogSystem;
        MessagePop.triggerDialog2 += TriggerDialogSystem2;


    }
    
    public void TriggerDialogSystem()
   {
       trigger = 10;
   }

    public void TriggerDialogSystem2()
   {
       trigger = 0;
   }*/

    void SetStyle(Subject Speaker1)

    {
        if (Speaker1 == null)
        {
            //sprite.sortingOrder = 0;
            SpeakerFaceDisplay1.color = new Color(1, 1, 1 , alphaLevel);
            Cahya.color = new Color(1, 1, 1 , alphaLevel);
            OnActive?.Invoke();

        }

        if(Speaker1.SubjectFace == null)
        {
            //sprite.sortingOrder = 0;
            SpeakerFaceDisplay1.color = new Color(1, 1, 1 , alphaLevel);
            Cahya.color = new Color(1, 1, 1 , alphaLevel);
            OnActive?.Invoke();
        }
        else
        {
            OnActive2?.Invoke();
            //sprite.sortingOrder = 5;
            SpeakerFaceDisplay1.sprite = Speaker1.SubjectFace;
            SpeakerFaceDisplay1.color = Color.white;
            Cahya.color = Color.white;
        }
        //DialogueBoxBorder.color = Speaker1.BorderColor;
        //DialogueBoxInner.color = Speaker1.InnerColor;
        SpeakerNameDisplay.SetText(Speaker1.SubjectName);
    }

        void SetStyle2(Subject Speaker2)

    {
        if (Speaker2 == null)
        {
            SpeakerFaceDisplay2.color = new Color(1, 1, 1, alphaLevel);
            Yanto.color = new Color(1, 1, 1, alphaLevel);

        }

        if(Speaker2.SubjectFace == null)
        {
            SpeakerFaceDisplay2.color = new Color(1, 1, 1, alphaLevel);
            Yanto.color = new Color(1, 1, 1, alphaLevel);
        }
        else
        {
            SpeakerFaceDisplay2.sprite = Speaker2.SubjectFace;
            SpeakerFaceDisplay2.color = Color.white;
            Yanto.color = Color.white;
        }
        //DialogueBoxBorder.color = Speaker2.BorderColor;
        //DialogueBoxInner.color = Speaker2.InnerColor;
        SpeakerNameDisplay.SetText(Speaker2.SubjectName);
    }

    

    IEnumerator PlayDialogue(string Dialogue)
    {
        CanContinue = false;
        DialogueDisplay.SetText(string.Empty);

        for (int i = 0; i < Dialogue.Length; i++)
        {
            DialogueDisplay.text += Dialogue[i];
            yield return new WaitForSeconds(1f / TextSpeed);
        }
                CanContinue = true;
    }
}


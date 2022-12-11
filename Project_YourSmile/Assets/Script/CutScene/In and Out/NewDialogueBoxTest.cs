using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class NewDialogueBoxTest : MonoBehaviour
{

    public DialogueSegment[] DialogueSegments;
    [Space]
    public Image SpeakerFaceDisplay1;
    public Image SpeakerFaceDisplay1Up;
    public Image SpeakerFaceDisplay2;
    public Image SpeakerFaceDisplay2Up;
    
    public Image DialogueBoxBorder;
    //public Image DialogueBoxInner;
    public Image SkipIndicator;
    public Image Cahya;
    public Image Cahya2;
    public Image Yanto;
    public Image Yanto2;
    public Animator DialogueAnimator;
    private bool StartDialogue = true;
    //[SerializeField] float x, y, z;
    
    [Space]
    public TextMeshProUGUI SpeakerNameDisplay;
    public TextMeshProUGUI SpeakerNameDisplay2;
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
                //gameObject.SetActive(false);s
                DialogueAnimator.SetTrigger("ChoiceIn");
                StartDialogue = true;
                //DialogueAnimator.SetTrigger("Exit");
               // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
               // DialogueIndex = 0;
                
                return;
            }

            SetStyle(DialogueSegments[DialogueIndex].Speaker1);
            SetStyle2(DialogueSegments[DialogueIndex].Speaker2);
            StartCoroutine(PlayDialogue(DialogueSegments[DialogueIndex].Dialogue));
        }
    }


    void SetStyle(Subject Speaker1)

    {
        if (Speaker1 == null)
        {
            //sprite.sortingOrder = 0;
            SpeakerFaceDisplay1.color = new Color(1, 1, 1 , alphaLevel);
            Cahya.color = new Color(1, 1, 1 , alphaLevel);

            OnActive?.Invoke();
           // DialogueAnimator.SetTrigger("Left");
            //Cahya.Recttransform.localScale = new Vector3(1f,1f,1f);
            

        }

        if(Speaker1.SubjectFace == null)
        {
            //sprite.sortingOrder = 0;
            SpeakerFaceDisplay1.color = new Color(1, 1, 1 , alphaLevel);
            Cahya.color = new Color(1, 1, 1 , alphaLevel);
            
            OnActive?.Invoke();
            //Cahya.Recttransform.localScale = new Vector3(1f,1f,1f);
            //DialogueAnimator.SetTrigger("Left");
        }
        else
        {

            OnActive2?.Invoke();
            //sprite.sortingOrder = 5;
        
            SpeakerFaceDisplay1Up.sprite = Speaker1.SubjectFace;
            SpeakerFaceDisplay1Up.color = Color.white;
            Cahya2.color = Color.white;
            Cahya2.sprite = Speaker1.borderFace;

            SpeakerFaceDisplay1.sprite = Speaker1.SubjectFace;
            SpeakerFaceDisplay1.color = Color.white;
            Cahya.color = Color.white;
            Cahya.sprite = Speaker1.borderFace;
            //DialogueAnimator.SetTrigger("LeftScaleUp");
            //Cahya.Recttransform.localScale = new Vector3(2f,2f,2f);
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
            //Yanto.Recttransform.localScale = new Vector3(1f,1f,1f);
            //DialogueAnimator.SetTrigger("Right");

        }

        if(Speaker2.SubjectFace == null)
        {
            SpeakerFaceDisplay2.color = new Color(1, 1, 1, alphaLevel);
            Yanto.color = new Color(1, 1, 1, alphaLevel);
            //Yanto.Recttransform.localScale = new Vector3(1f,1f,1f);
           // DialogueAnimator.SetTrigger("Right");
        }
        else
        {
            
            SpeakerFaceDisplay2.sprite = Speaker2.SubjectFace;
            SpeakerFaceDisplay2.color = Color.white;
            Yanto.color = Color.white;
            Yanto.sprite = Speaker2.borderFace;


            
            SpeakerFaceDisplay2Up.sprite = Speaker2.SubjectFace;
            SpeakerFaceDisplay2Up.color = Color.white;
            Yanto2.color = Color.white;
            Yanto2.sprite = Speaker2.borderFace;
           // DialogueAnimator.SetTrigger("RightScaleUp");
            //Yanto.Recttransform.localScale = new Vector3(2f,2f,2f);
        }
        //DialogueBoxBorder.color = Speaker2.BorderColor;
        //DialogueBoxInner.color = Speaker2.InnerColor;
        SpeakerNameDisplay2.SetText(Speaker2.SubjectName);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


namespace DialogueSystem
{
    public class DialogueLine : DialogueBaseClass
    {


        private Text textHolder;

        //[Header ("Text Option")]
        [SerializeField] private string input;
        //[SerializeField]private Color textColor;
        //[SerializeField]private Font textFont;

        [Header ("TimeParameter")]
        [SerializeField] private float delay;

       // [Header ("Character Image")]
      //  [SerializeField] private Sprite CharacterSprite;
        //[SerializeField] private Image imageHolder;

        private void Awake()
        {
            textHolder = GetComponent<Text>();
            textHolder.text = "";
            StartCoroutine(WriteText(input,textHolder,delay));
            //imageHolder.sprite = CharacterSprite;
            //imageHolder.preserveAspect = true;

            //StartCoroutine(WriteText(input,textHolder, textColor, textFont));

        }
        
    }
}




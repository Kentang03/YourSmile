using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace DialogueSystem
{
public class DialogueBaseClass : MonoBehaviour
{
    //protected IEnumerator WriteText(string input, Text textHolder, Color textColor, Font textFont)
    protected IEnumerator WriteText(string input, Text textHolder, float delay)
    {
        //textHolder.color = textColor;
        //textHolder.font = textFont;

        for (int i = 0; i < input.Length; i++)
        {
            textHolder.text += input[i];
            yield return new WaitForSeconds(delay);
        }

    }

}
}


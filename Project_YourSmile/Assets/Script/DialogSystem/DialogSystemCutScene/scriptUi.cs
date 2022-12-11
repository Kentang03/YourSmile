using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
public class scriptUi : MonoBehaviour
{
    public Animator DialogueAnimator;
    public void LoadToScene()
    {
        DialogueAnimator.SetTrigger("ChoiceOut");
    } 

}


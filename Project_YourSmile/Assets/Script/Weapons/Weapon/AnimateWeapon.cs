using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]

public class AnimateWeapon : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float transitionTime = 2f;
    [SerializeField] private float countdown = 0f;
    

    private bool isOpen = false;

    //private void Awake()
    //{      
    //    animator = GetComponent<Animator>();
    //}

    //private void OnEnable()
    //{
    //    //animator.SetBool(GameSettings.BookOpen, isOpen);
    //}

    public void ShootCooldown(float countdown)
    {
        countdown -= Time.deltaTime;       
        
    }

    public void OpenBook()
    {
        if (!isOpen)
        {
            isOpen = true;
            //animator.SetBool(GameSettings.BookOpen, true);
            animator.SetTrigger("Trigger_BookOpening");
        }
    }

    public void CloseBook()
    {
        if(countdown < 0f)
        {
            countdown = GetCountdown(); 
    
            isOpen = false;
            //animator.SetBool(GameSettings.BookOpen, false);
            animator.SetTrigger("Trigger_BookClosing");
        }
    }

    public void SetCountdown()
    {
        countdown = transitionTime;
    }
    public float GetCountdown()
    {
        return countdown;
    }

    
}

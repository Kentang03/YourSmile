using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyProp : MonoBehaviour
{
    public EnemyDetailsSO enemyDetails;
    public static event Action OnTargetDeath;


    //efek mati
    //public GameObject deathEffect;
    public int darah;
    public void Awake()
    {
        
    }

    public void TakeDamage (int damage)
    {
    darah -= damage;

        if(darah <= 0)
        {
            Die();
            OnTargetDeath?.Invoke();
        }

    }

    void Die()
    {
        //Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent]

public class Health : MonoBehaviour
{
    private int startingHealth;
    private int currentHealth;
    private HealthEvent healthEvent;
    private Player player;

    [HideInInspector] public bool isDamageable = true;
    [HideInInspector] public Enemy enemy;

    private void Awake()
    {
        //Load Components
        healthEvent = GetComponent<HealthEvent>();
    }

    private void Start()
    {
        //Trigger a health event for UI update
        CallHealthEvent(0);

        //Attempt to Load enemy/ player components
        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();
    }
    
    public void TakeDamage(int damageAmount)
    {
        bool isDashing = false;
        if(player != null)
            isDashing = player.playerControl.isPlayerDashing;

        if (isDamageable && !isDashing)
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);
        }

        if (isDamageable && isDashing)
        {
            Debug.Log("Dodge Bullet by Dashing");
        }
    }

    private void CallHealthEvent(int damageAmount)
    {
        //Trigger health event
        healthEvent.CallHealthChangedEvent((float)currentHealth/ (float)startingHealth, currentHealth, damageAmount);

    }

    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }

    public int GetStartingHealth()
    {
        return startingHealth;
    }
}


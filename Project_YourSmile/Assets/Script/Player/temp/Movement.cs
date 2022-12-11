using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    //[SerializeField] private float movementSpeed = 3.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    public Rigidbody2D rb2d;
    private Vector2 moveInput;
    public Animator animator;
    public ParticleSystem dust;
 
    private float activeMoveSpeed;
    public float dashSpeed;
    public float dashLength = .5f, dashCooldown = 1f;
    private float dashCounter;
    private float dashCoolCounter;
    
    void Start() 
    {
        activeMoveSpeed = moveSpeed;
        animator = GetComponent<Animator>();
    }
  
    void Update()
    {
        dash();
        
    }

   
     void FixedUpdate()
    {
         if (moveInput != Vector2.zero)
                    {
                        var xMovement = moveInput.x * moveSpeed * Time.deltaTime;
                        this.transform.Translate(new Vector3(xMovement, 0), Space.World);
                    }
    }

    void dash()
    {
        CreateDust();
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        moveInput.Normalize();

        rb2d.velocity = moveInput * activeMoveSpeed;

        animator.SetFloat("Speed", Mathf.Abs(moveInput.magnitude * moveSpeed));
        bool flipped = moveInput.x < 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));


       if (Input.GetKeyDown(KeyCode.Space))
        {
            if(dashCoolCounter <= 0 && dashCounter <= 0)
            {
                activeMoveSpeed = dashSpeed;
                dashCounter = dashLength;
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;

            if(dashCounter <= 0)
            {
                activeMoveSpeed = moveSpeed;
                dashCoolCounter = dashCooldown;
            }
        }

        if(dashCoolCounter > 0)
        {
            dashCoolCounter -= Time.deltaTime;
        }
    }

    void CreateDust()
    {
        dust.Play();
    }

}
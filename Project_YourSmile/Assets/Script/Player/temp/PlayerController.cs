using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 3.0f;
    private Vector4 movement;

    private Animator animator;

    public Rigidbody2D rb;
    public Camera cam;
    //Vector2 mousePos;

   
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        movement = new Vector4(Input.GetAxis("Horizontal"), 0).normalized;
        animator.SetFloat("Speed", Mathf.Abs(movement.magnitude * movementSpeed));

        bool flipped = movement.x < 0;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, flipped ? 180f : 0f, 0f));

    }

    void FixedUpdate()
    {
        //Vector2 lookDir = mousePos - rb.position;

       // float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 90f;
        //rb.rotation = angle;

        if (movement != Vector4.zero)
                    {
                        var xMovement = movement.x * movementSpeed * Time.deltaTime;
                        this.transform.Translate(new Vector3(xMovement, 0), Space.World);
                    }
    }
}

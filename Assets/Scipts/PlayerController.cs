using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    private float moveInput;
    private Rigidbody2D rb;
    private bool facingRight = true;
    private bool isGrounded;
    public Transform feetPos;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private int extraJumps;
    public int extraJumpsValue;
    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    public ParticleSystem dust;
    public VerticalPlatforms cameraShake;
    public float shakeDuration = 0.2f;
    public float shakeMagnitude = 0.2f;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumps = extraJumpsValue;
    }
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius,whatIsGround);
        moveInput = Input.GetAxis("Horizontal");
        Debug.Log(moveInput);
        rb.velocity = new Vector2(moveInput*speed,rb.velocity.y);
        if (facingRight == false && moveInput > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
        {
            Flip();
        } 
    }
    private void Update()
    {
        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)&& extraJumps > 0)
        {
            rb.velocity = Vector2.up*jumpForce;
            extraJumps--;
            CreateDust();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)&& extraJumps == 0 && isGrounded == true )
        {      
            rb.velocity = Vector2.down*jumpForce;
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
        CreateDust();
    }
    void LateUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            rb.velocity = Vector2.up * jumpForce;

            
        }
        if (Input.GetKey(KeyCode.Space) && isJumping)
        {
            if (jumpTimeCounter > 0)
            {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
                
            }
            else
            {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            isJumping = false;
        }  
    }
    void OnCollisionEnter2D(Collision2D collision)

    {

        if (collision.gameObject.CompareTag("Collider"))

        {

            if (collision.relativeVelocity.magnitude > 1) // Adjust threshold if needed

            {

                cameraShake.TriggerShake(shakeDuration, shakeMagnitude);

            }

        }

    }
    void CreateDust()
    {    
            dust.Play();
    }
}

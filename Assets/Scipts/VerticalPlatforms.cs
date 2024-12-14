using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatforms : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private float inputHorizontal;
    private float inputVertical;
    public float distance;
    public LayerMask whatIsLadder;
    private bool isClimbing;
    private float shakeDuration = 0f; // Ensure this starts at 0
    public float shakeMagnitude = 0.1f;
    public float dampingSpeed = 1f;
    private Vector3 originalPosition;

    void Awake()

    {

        originalPosition = transform.position;

    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        effector = GetComponent<PlatformEffector2D>();
    }
    private void FixedUpdate()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputHorizontal * speed, rb.velocity.y);
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        if (hitInfo.collider != null)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                isClimbing = true;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
            {
                isClimbing = false;
            }
        }

        if (isClimbing == true && hitInfo.collider != null)
        {
            inputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x, inputVertical * speed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 5;
        }
    }

    private PlatformEffector2D effector;
    public float waitTime;
    private bool canDrop = true; // ตรวจสอบว่า player สามารถลงได้หรือไม่
    private void Update()
    {
        if (canDrop && (Input.GetKeyDown(KeyCode.DownArrow)))
        {
            effector.rotationalOffset = 180f; // หมุนแพลตฟอร์มลง
            StartCoroutine(ResetPlatform()); // เรียก coroutine เพื่อรีเซ็ตแพลตฟอร์ม
            canDrop = false; // ปิดการอนุญาตให้ตกลงอีกจนกว่าจะปล่อยปุ่ม
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            canDrop = true; // อนุญาตให้ player สามารถตกลงได้อีกครั้ง
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            effector.rotationalOffset = 0; // รีเซ็ตให้แพลตฟอร์มกลับสู่สภาพเดิม
        }
        if (shakeDuration > 0)

        {

            // Apply shake effect

            transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;



            // Decrease shake duration

            shakeDuration -= Time.deltaTime * dampingSpeed;



            // Reset position when duration ends

            if (shakeDuration <= 0)

            {

                shakeDuration = 0f;

                transform.position = originalPosition;

            }

        }
    }
    public void TriggerShake(float duration, float magnitude)

    {

        shakeDuration = duration;

        shakeMagnitude = magnitude;

    }

    private IEnumerator ResetPlatform()
    {
        yield return new WaitForSeconds(0.2f); // รอเวลาสั้น ๆ ก่อนรีเซ็ตแพลตฟอร์ม
        effector.rotationalOffset = 0;
    }

}
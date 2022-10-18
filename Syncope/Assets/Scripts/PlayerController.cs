using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public int extraJumpsValue;
    public SceneController sceneController;
    public bool enableDash;
    public float dashSpeed;
    public int dashValue;

    private float moveInput;
    private bool facingRight = true;
    private bool isGrounded;
    private int extraJumps;
    private Rigidbody2D rb;
    private bool canHide = false;
    private SpriteRenderer rend;
    private HealthBar health;
    private float dash = 0f;
    private int extraDashes;

    private void Start()
    {
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        health = GetComponent<HealthBar>();
        if (enableDash)
            extraDashes = dashValue;
    }

    private void Update()
    {
        if (isGrounded)
        {
            extraJumps = extraJumpsValue;
            extraDashes = dashValue;
        }
            
        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded)
            rb.velocity = Vector2.up * jumpForce;
        if (enableDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && extraDashes >=0)
            {
                dash = dashSpeed;
                extraDashes--;
            }
        }
        if (transform.position.y < -5f)
            sceneController.RestartLevel();
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        rb.AddForce(new Vector2(moveInput * dash, 0), ForceMode2D.Impulse);
        if (dash > 0f)
            dash = 0f;
        if ((!facingRight && moveInput > 0) || (facingRight && moveInput < 0))
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void setHiding(bool enableHiding)
    {
        canHide = enableHiding;
        if (canHide)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            rend.sortingOrder = 0;
            health.setFillCoefficient(-10f);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(9, 10, false);
            rend.sortingOrder = 2;
            health.setFillCoefficient(1f);
        }
    }

    public void affectedByEnemy(bool decreasing)
    {
        if (decreasing)
            health.setFillCoefficient(3f);//сильнее убывает хп
        else
            health.setFillCoefficient(1f);
    }
       
}

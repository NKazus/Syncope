using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public bool isFalling = false;//врубать этот скрипт или нет
    public SpriteRenderer spriteRenderer;//компонент дочернего объекта, то есть самого спрайта платформы
    public Sprite fallSprite;//альтернативная версия спрайта для падающей платформы

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private bool movingBack;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        //spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            if (movingBack)
            {
                transform.position = Vector2.MoveTowards(transform.position, initialPosition, 20f * Time.deltaTime);
            }

            if (transform.position.y == initialPosition.y)
            {
                movingBack = false;
            }
        }
        
    }

    private void GoBack()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        movingBack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFalling)
        {
            if (collision.gameObject.name.Equals("Character") && !movingBack)
            {
                Invoke("Fall", 1f);
            }
        }    
    }

    private void Fall()
    {
        rb.isKinematic = false;
        Invoke("GoBack",1f);
    }

    public void SetFalling()
    {
        isFalling = true;
        //spriteRenderer.color = Color.blue;
        spriteRenderer.sprite = fallSprite;
    }
}

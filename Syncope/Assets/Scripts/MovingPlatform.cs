using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Sprite moveSprite;
    public SpriteRenderer spriteRenderer;

    private Vector2 topPosition;
    private Vector2 bottomPosition;
    private bool moveUp = false;
    //private bool moveDown = false;
    private bool isMoving = false;
    private float movingSpeed;

    // Update is called once per frame
    private void Update()
    {
        if (isMoving)
        {
            print("isMoving");

            if (transform.position.y >= topPosition.y)
            {
                moveUp = false;
                //moveDown = true;
            }
            if (transform.position.y <= bottomPosition.y)
            {
                //moveDown = false;
                moveUp = true;
            }
            if (moveUp)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + movingSpeed * Time.deltaTime);
                print("moveUp");
                
            }
            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - movingSpeed * Time.deltaTime);
            }
        }
    }

    public void SetMoving(float minHeight, float maxHeight, float moveSpeed)
    {
        isMoving = true;
        moveUp = true;
        topPosition = new Vector2(transform.position.x, maxHeight + 0.5f);
        bottomPosition = new Vector2(transform.position.x, minHeight - 0.5f);
        movingSpeed = moveSpeed;
        spriteRenderer.sprite = moveSprite;
    }
}

using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private bool isMoving = false;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite moveSprite;

    private Vector2 _topPosition;
    private Vector2 _bottomPosition;
    private bool _moveUp = false;
    private float _movingSpeed;

    private void Update()
    {
        if (isMoving)
        {

            if (transform.position.y >= _topPosition.y)
            {
                _moveUp = false;
            }
            if (transform.position.y <= _bottomPosition.y)
            {
                _moveUp = true;
            }
            if (_moveUp)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + _movingSpeed * Time.deltaTime);
                
            }
            else
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - _movingSpeed * Time.deltaTime);
            }
        }
    }

    public void SetMoving(float minHeight, float maxHeight, float moveSpeed)
    {
        isMoving = true;
        _moveUp = true;
        _topPosition = new Vector2(transform.position.x, maxHeight + 0.5f);
        _bottomPosition = new Vector2(transform.position.x, minHeight - 0.5f);
        _movingSpeed = moveSpeed;
        spriteRenderer.sprite = moveSprite;
    }
}

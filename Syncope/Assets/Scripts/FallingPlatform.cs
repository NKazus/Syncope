using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private bool _isFalling = false;//врубать этот скрипт или нет
    [SerializeField] private SpriteRenderer _spriteRenderer;//компонент дочернего объекта, то есть самого спрайта платформы
    [SerializeField] private Sprite _fallSprite;//альтернативная версия спрайта для падающей платформы

    private Rigidbody2D _platformRigidBody;
    private Vector2 _initialPosition;
    private bool _movingBack;
    
    private void Start()
    {
        _platformRigidBody = GetComponent<Rigidbody2D>();
        _initialPosition = transform.position;
    }

    private void Update()
    {
        if (_isFalling)
        {
            if (_movingBack)
            {
                transform.position = Vector2.MoveTowards(transform.position, _initialPosition, 20f * Time.deltaTime);
            }

            if (transform.position.y == _initialPosition.y)
            {
                _movingBack = false;
            }
        }
        
    }
    private void GoBack()
    {
        _platformRigidBody.velocity = Vector2.zero;
        _platformRigidBody.isKinematic = true;
        _movingBack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isFalling)
        {
            if (collision.gameObject.name.Equals("Character") && !_movingBack)
            {
                Invoke("Fall", 1f);
            }
        }    
    }

    private void Fall()
    {
        _platformRigidBody.isKinematic = false;
        Invoke("GoBack",1f);
    }

    public void SetFalling()
    {
        _isFalling = true;
        _spriteRenderer.sprite = _fallSprite;
    }
}

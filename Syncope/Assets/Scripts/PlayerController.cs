using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkRadius = 0.5f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private int extraJumpsValue = 1; //2+.2
    [SerializeField] private bool enableDash = false; //4+.true
    [SerializeField] private float dashSpeed = 50f; //4+.50
    [SerializeField] private int dashValue = 0; //5+.1

    private float _moveInput;
    private bool _facingRight = true;
    private bool _isGrounded;
    private int _extraJumps;
    private Rigidbody2D _playerRigidBody;
    private bool _canHide = false;
    private SpriteRenderer _rend;
    private HealthBar _health;
    private float _dash = 0f;
    private int _extraDashes;

    private void Awake()
    {
        GlobalEventManager.HealthAffectingEvent.AddListener(AffectedByEnemy);
    }
    private void Start()
    {
        _extraJumps = extraJumpsValue;
        _playerRigidBody = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();
        _health = GetComponent<HealthBar>();
        if (enableDash)
            _extraDashes = dashValue;
    }

    private void Update()
    {
        if (_isGrounded)
        {
            _extraJumps = extraJumpsValue;
            _extraDashes = dashValue;
        }
            
        if (Input.GetKeyDown(KeyCode.Space) && _extraJumps > 0)
        {
            _playerRigidBody.velocity = Vector2.up * jumpForce;
            _extraJumps--;
        }
        else if(Input.GetKeyDown(KeyCode.Space) && _extraJumps == 0 && _isGrounded)
            _playerRigidBody.velocity = Vector2.up * jumpForce;
        if (enableDash)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && _extraDashes >= 0)
            {
                _dash = dashSpeed;
                _extraDashes--;
            }
        }
        if (transform.position.y < -5f)
            GlobalEventManager.RestartScene();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        _moveInput = Input.GetAxis("Horizontal");
        _playerRigidBody.velocity = new Vector2(_moveInput * speed, _playerRigidBody.velocity.y);
        _playerRigidBody.AddForce(new Vector2(_moveInput * _dash, 0), ForceMode2D.Impulse);
        if (_dash > 0f)
            _dash = 0f;
        if ((!_facingRight && _moveInput > 0) || (_facingRight && _moveInput < 0))
            Flip();
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    public void SetHiding(bool enableHiding)
    {
        _canHide = enableHiding;
        if (_canHide)
        {
            Physics2D.IgnoreLayerCollision(9, 10, true);
            _rend.sortingOrder = 0;
            _health.setFillCoefficient(-10f);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(9, 10, false);
            _rend.sortingOrder = 2;
            _health.setFillCoefficient(1f);
        }
    }

    private void AffectedByEnemy(bool decreasing)
    {
        if (decreasing)
            _health.setFillCoefficient(3f);//сильнее убывает хп
        else
            _health.setFillCoefficient(1f);
    }
       
}

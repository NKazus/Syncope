//fixed
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _leftBorder = 6f;
    [SerializeField] private float _rightBorder = 11f;
    private AudioSource _enemySound;

    private void Start()
    {
        _enemySound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime);
        if (transform.position.x >= _rightBorder)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (transform.position.x <= _leftBorder)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            GlobalEventManager.AffectPlayerHealth(true);
            _enemySound.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            GlobalEventManager.AffectPlayerHealth(false);
            _enemySound.Stop();
        }
    }

    public void SetEnemyParameters(float speed, float lBorder, float rBorder)
    {
        this._speed = speed;
        _leftBorder = lBorder;
        _rightBorder = rBorder;
    }
}

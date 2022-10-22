using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float enemySpeed = 1f;
    [SerializeField] private float leftBorder = 6f;
    [SerializeField] private float rightBorder = 11f;
    private AudioSource _enemySound;

    private void Start()
    {
        _enemySound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector2.left * enemySpeed * Time.deltaTime);
        if (transform.position.x >= rightBorder)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        if (transform.position.x <= leftBorder)
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
        enemySpeed = speed;
        leftBorder = lBorder;
        rightBorder = rBorder;
    }
}

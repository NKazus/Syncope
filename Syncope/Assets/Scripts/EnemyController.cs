using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController player;

    public float speed = 0;
    public float leftBorder;
    public float rightBorder;

    private AudioSource enemySound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemySound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
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
            player.affectedByEnemy(true);
            enemySound.Play();
            print("enemy_on");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            player.affectedByEnemy(false);
            enemySound.Stop();
            print("enemy_off");
        }
    }

    public void SetEnemyParameters(float speed, float lBorder, float rBorder)
    {
        this.speed = speed;
        leftBorder = lBorder;
        rightBorder = rBorder;
    }
}

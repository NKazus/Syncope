using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public int healthReduction;//какой кусок хп сносит ловушка

    private Rigidbody2D rb;
    private HealthBar hb;
    private bool volumeCheck = false;
    private Vector2 initialPosition;//начальная высота по y
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hb = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBar>();
        initialPosition = transform.position;
        Physics2D.IgnoreLayerCollision(8, 10, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (volumeCheck)//если ловушка пришла в движение, проверяем высоту
        {
            if (transform.position.y <= -5f)
            {
                RestorePosition();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            hb.healthReduce(healthReduction);
        }
    }

    public void InitiateVolumeCheck()
    {
        volumeCheck = true;
    }

    private void RestorePosition()
    {
        rb.isKinematic = true;
        transform.position = initialPosition;
        volumeCheck = false;
    }
}

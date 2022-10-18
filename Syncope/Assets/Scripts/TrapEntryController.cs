using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEntryController : MonoBehaviour
{
    private Rigidbody2D rb;
    private TrapController trap;
    private float gravity = 1f;
    private bool setGravity = true;
    private AudioSource trapFallingSound;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetChild(0).GetComponent<Rigidbody2D>();
        trap = transform.GetChild(0).GetComponent<TrapController>();
        trapFallingSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            rb.isKinematic = false;
            if (setGravity)
            {
                rb.gravityScale = gravity;
                setGravity = false;
            }
            trap.InitiateVolumeCheck();
            trapFallingSound.PlayOneShot(trapFallingSound.clip);
        }
    }

    public void SetTrapGravity(float initialGravity)
    {
        gravity = initialGravity;
    }
}

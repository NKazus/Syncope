using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceController : MonoBehaviour
{
    public int healthIncreasement;
    public GameObject essenceParent;
    public bool dontUsePool;

    private HealthBar hb;

    // Start is called before the first frame update
    void Start()
    {
        hb = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            hb.healthIncrease(healthIncreasement);
            if (!dontUsePool)
            {
                transform.GetChild(0).GetComponent<Blinking>().StopBlinking();
                PoolManager.putGameObjectToPool(essenceParent);
            }
        }
    }
    
}

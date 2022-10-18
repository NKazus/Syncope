using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePlatform : MonoBehaviour
{
    public bool controlBlinking = false;
    public bool exitSound = false;

    private GameObject platform;
    private AudioSource exitClip;
    
    // Start is called before the first frame update
    void Start()
    {
        platform = transform.GetChild(0).gameObject;
        if (exitSound)
            exitClip = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            platform.SetActive(true);
            if(exitSound)
                exitClip.Play();
            if (controlBlinking)
                platform.transform.GetChild(0).GetComponent<Blinking>().StartBlinking();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            platform.SetActive(false);
            if (exitSound)
                exitClip.Stop();
            if (controlBlinking)
                platform.transform.GetChild(0).GetComponent<Blinking>().StopBlinking();
        }
    }
}

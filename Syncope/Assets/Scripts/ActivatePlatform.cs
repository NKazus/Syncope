//fixed
using UnityEngine;

public class ActivatePlatform : MonoBehaviour
{
    public bool ControlBlinking = false;
    public bool ExitSound = false;

    private GameObject _platform;
    private AudioSource _exitClip;
    
    private void Start()
    {
        _platform = transform.GetChild(0).gameObject;
        if (ExitSound)
            _exitClip = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _platform.SetActive(true);
            if(ExitSound)
                _exitClip.Play();
            if (ControlBlinking)
                _platform.transform.GetChild(0).GetComponent<Blinking>().StartBlinking();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _platform.SetActive(false);
            if (ExitSound)
                _exitClip.Stop();
            if (ControlBlinking)
                _platform.transform.GetChild(0).GetComponent<Blinking>().StopBlinking();
        }
    }
}

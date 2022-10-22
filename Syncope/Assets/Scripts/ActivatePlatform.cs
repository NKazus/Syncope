using UnityEngine;

public class ActivatePlatform : MonoBehaviour
{
    [SerializeField] private bool controlBlinking = false;
    [SerializeField] private bool exitSound = false;

    private GameObject _platform;
    private AudioSource _exitClip;
    
    private void Start()
    {
        _platform = transform.GetChild(0).gameObject;
        if (exitSound)
            _exitClip = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _platform.SetActive(true);
            if(exitSound)
                _exitClip.Play();
            if (controlBlinking)
                _platform.transform.GetChild(0).GetComponent<Blinking>().StartBlinking();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _platform.SetActive(false);
            if (exitSound)
                _exitClip.Stop();
            if (controlBlinking)
                _platform.transform.GetChild(0).GetComponent<Blinking>().StopBlinking();
        }
    }
}

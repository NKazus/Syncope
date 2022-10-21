using UnityEngine;

public class ActivatePlatform : MonoBehaviour
{
    [SerializeField] private bool _controlBlinking = false;
    [SerializeField] private bool _exitSound = false;

    private GameObject _platform;
    private AudioSource _exitClip;
    
    private void Start()
    {
        _platform = transform.GetChild(0).gameObject;
        if (_exitSound)
            _exitClip = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _platform.SetActive(true);
            if(_exitSound)
                _exitClip.Play();
            if (_controlBlinking)
                _platform.transform.GetChild(0).GetComponent<Blinking>().StartBlinking();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _platform.SetActive(false);
            if (_exitSound)
                _exitClip.Stop();
            if (_controlBlinking)
                _platform.transform.GetChild(0).GetComponent<Blinking>().StopBlinking();
        }
    }
}

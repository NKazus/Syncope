//fixed
using UnityEngine;

public class TrapEntryController : MonoBehaviour
{
    private Rigidbody2D _trapRigidBody;
    private TrapController _trap;
    private float _gravity = 1f;
    private bool _setGravity = true;
    private AudioSource _trapFallingSound;
    
    private void Start()
    {
        _trapRigidBody = transform.GetChild(0).GetComponent<Rigidbody2D>();
        _trap = transform.GetChild(0).GetComponent<TrapController>();
        _trapFallingSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _trapRigidBody.isKinematic = false;
            if (_setGravity)
            {
                _trapRigidBody.gravityScale = _gravity;
                _setGravity = false;
            }
            _trap.InitiateVolumeCheck();
            _trapFallingSound.PlayOneShot(_trapFallingSound.clip);
        }
    }

    public void SetTrapGravity(float initialGravity)
    {
        _gravity = initialGravity;
    }
}

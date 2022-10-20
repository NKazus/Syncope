//fixed
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public int HealthReduction;//какой кусок хп сносит ловушка

    private Rigidbody2D _trapRigidBody;
    private bool _volumeCheck = false;
    private Vector2 _initialPosition;//начальная высота по y
   
    void Start()
    {
        _trapRigidBody = GetComponent<Rigidbody2D>();
        _initialPosition = transform.position;
        Physics2D.IgnoreLayerCollision(8, 10, true);
    }

    void Update()
    {
        if (_volumeCheck)//если ловушка пришла в движение, проверяем высоту
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
           GlobalEventManager.ChangePlayerHealth((-1)*HealthReduction);
        }
    }

    public void InitiateVolumeCheck()
    {
        _volumeCheck = true;
    }

    private void RestorePosition()
    {
        _trapRigidBody.isKinematic = true;
        transform.position = _initialPosition;
        _volumeCheck = false;
    }
}

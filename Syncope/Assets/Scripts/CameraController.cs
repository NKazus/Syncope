using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _damping = 1.5f;
    [SerializeField] private Vector2 _offset = new Vector2(2f, 1f);

    private bool _isLeft;
    private Transform _player;
    private int _lastX;
    private float _upperLimit;
    private float _bottomLimit;

    private void Start()
    {
        _offset = new Vector2(Mathf.Abs(_offset.x), _offset.y);
        _upperLimit = GameObject.FindGameObjectWithTag("MaxHeightPoint").transform.position.y;//центр камеры не может выйти за самую высокую
        _bottomLimit = GameObject.FindGameObjectWithTag("PlatformGenerator").transform.position.y;//и самую низкую точку спавна платформ
        FindPlayer(_isLeft);
    }

    public void FindPlayer(bool playerIsLeft)
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _lastX = Mathf.RoundToInt(_player.position.x);
        if (playerIsLeft)
            transform.position = new Vector3(_player.position.x - _offset.x, _player.position.y + _offset.y, transform.position.z);
        else
            transform.position = new Vector3(_player.position.x + _offset.x, _player.position.y + _offset.y, transform.position.z);
    }

    private void Update()
    {
        if (_player)
        {
            int currentX = Mathf.RoundToInt(_player.position.x);
            if (currentX > _lastX)
                _isLeft = false;
            else if (currentX < _lastX)
                _isLeft = true;
            _lastX = Mathf.RoundToInt(_player.position.x);

            Vector3 target;
            if(_isLeft)
                target = new Vector3(_player.position.x - _offset.x, _player.position.y + _offset.y, transform.position.z);
            else
                target = new Vector3(_player.position.x + _offset.x, _player.position.y + _offset.y, transform.position.z);

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, _damping * Time.deltaTime);
            transform.position = currentPosition;
        }

        transform.position = 
            new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _bottomLimit, _upperLimit), transform.position.z);
    }
}

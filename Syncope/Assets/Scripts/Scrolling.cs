using UnityEngine;

public class Scrolling : MonoBehaviour
{
    [SerializeField] private float backgroundSize = 57.5f;
    [SerializeField] private float parallaxSpeed = 0.5f;
    [SerializeField] private bool scroll = true;
    [SerializeField] private bool parallax = true;

    private Transform _cameraTransform;
    private Transform[] _layers;
    private float _viewZone = 10;
    private int _leftIndex;
    private int _rightIndex;
    private float _lastCameraX;

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraX = _cameraTransform.position.x;
        _layers = new Transform[transform.childCount];
        for(int i = 0; i < transform.childCount; i++)
        {
            _layers[i] = transform.GetChild(i);
        }
        _leftIndex = 0;
        _rightIndex = _layers.Length - 1;
    }

    private void Update()
    {
        if (parallax)
        {
            float deltaX = _cameraTransform.position.x - _lastCameraX;
            transform.position += Vector3.right * (deltaX * parallaxSpeed);
        }
        _lastCameraX = _cameraTransform.position.x;
        if (scroll)
        {
            if (_cameraTransform.position.x < (_layers[_leftIndex].transform.position.x + _viewZone / 2f))
            {
                ScrollLeft();
            }
            if (_cameraTransform.position.x > (_layers[_rightIndex].transform.position.x + _viewZone / 2f))
            {
                ScrollRight();
            }
        }
    }

    private void ScrollLeft()
    {
        int lastRight = _rightIndex;
        _layers[_rightIndex].position = Vector3.right * (_layers[_leftIndex].position.x - backgroundSize);
        _leftIndex = _rightIndex;
        _rightIndex--;
        if (_rightIndex < 0)
            _rightIndex = _layers.Length - 1;
    }

    private void ScrollRight()
    {
        int lastLeft = _leftIndex;
        _layers[_leftIndex].position = Vector3.right * (_layers[_rightIndex].position.x + backgroundSize);
        _rightIndex = _leftIndex;
        _leftIndex++;
        if (_leftIndex == _layers.Length)
            _leftIndex = 0;
    }
}

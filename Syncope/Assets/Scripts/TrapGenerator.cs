//fixed
using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _traps;//список префабов ловушек
    [SerializeField] private float _trapDelta = 5f;//смещение спавна ловушки
    [SerializeField] private float _minGravity = 2f;
    [SerializeField] private float _maxGravity = 4f;

    private float _gravity;//скорость падения ловушки
    private int _trapSelector;
    private float _platformHeight;
    private float _spawnPositionX;//середина платформы
    private float _spawnPositionY;//середина платформы
    private float _offset;

    public void SetSpawnPosition(float posX, float posY)
    {
        _spawnPositionX = posX;
        _spawnPositionY = posY;
    }

    public void SetPlatformHeight(float platHeight)
    {
        _platformHeight = platHeight/2f;
    }

    public void SpawnTrap()
    {
        _trapSelector = Random.Range(0, _traps.Length);
        _gravity = Random.Range(_minGravity, _maxGravity);
        _offset = Random.Range(-_trapDelta, _trapDelta);
        GameObject currentTrap = Instantiate(_traps[_trapSelector],
            new Vector3(_spawnPositionX + _offset, _spawnPositionY + _platformHeight + _traps[_trapSelector].GetComponent<BoxCollider2D>().size.y/2f, 0), transform.rotation);
        currentTrap.transform.GetChild(0).GetComponent<TrapEntryController>().SetTrapGravity(_gravity);
    }
}

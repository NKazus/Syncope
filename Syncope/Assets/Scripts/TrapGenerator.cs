using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] traps;//список префабов ловушек
    [SerializeField] private float trapDelta = 5f;//смещение спавна ловушки
    [SerializeField] private float minGravity = 2f;
    [SerializeField] private float maxGravity = 4f;

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
        _trapSelector = Random.Range(0, traps.Length);
        _gravity = Random.Range(minGravity, maxGravity);
        _offset = Random.Range(-trapDelta, trapDelta);
        GameObject currentTrap = Instantiate(traps[_trapSelector],
            new Vector3(_spawnPositionX + _offset, _spawnPositionY + _platformHeight + traps[_trapSelector].GetComponent<BoxCollider2D>().size.y/2f, 0), transform.rotation);
        currentTrap.transform.GetChild(0).GetComponent<TrapEntryController>().SetTrapGravity(_gravity);
    }
}

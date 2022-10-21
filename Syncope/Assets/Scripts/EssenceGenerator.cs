using UnityEngine;

public class EssenceGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _essences;
    [SerializeField] private float _essenceDelta = 8f;
    [SerializeField] private float _minHeight = 2f;//высота над платформой
    [SerializeField] private float _maxHeight = 4f;

    private int _essenceSelector;
    private float _offset;
    private float _spawnPositionX;//середина платформы
    private float _spawnPositionY;//середина платформы
    private float _platformHeight;
    private float _height;

    public void SetSpawnPosition(float spawnX, float spawnY)
    {
        _spawnPositionX = spawnX;
        _spawnPositionY = spawnY;
    }

    public void SetPlatformHeight(float platHeight)
    {
        _platformHeight = platHeight / 2f;
    }

    public void SpawnEssence()
    {
        _essenceSelector = Random.Range(0, _essences.Length);
        _offset = Random.Range(-_essenceDelta, _essenceDelta);
        _height = Random.Range(_minHeight, _maxHeight);
        GameObject currentEssence = PoolManager.getGameObjectFromPool(_essences[_essenceSelector]);
        currentEssence.transform.position = new Vector3(_spawnPositionX + _offset, _spawnPositionY + _platformHeight + _height, 0);
    }
}

using UnityEngine;

public class EssenceGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] essences;
    [SerializeField] private float essenceDelta = 8f;
    [SerializeField] private float minHeight = 2f;//высота над платформой
    [SerializeField] private float maxHeight = 4f;

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
        _essenceSelector = Random.Range(0, essences.Length);
        _offset = Random.Range(-essenceDelta, essenceDelta);
        _height = Random.Range(minHeight, maxHeight);
        GameObject currentEssence = PoolManager.getGameObjectFromPool(essences[_essenceSelector]);
        currentEssence.transform.position = new Vector3(_spawnPositionX + _offset, _spawnPositionY + _platformHeight + _height, 0);
    }
}

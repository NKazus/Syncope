using UnityEngine;

public class EnemyGenerator:MonoBehaviour
{
    [SerializeField] private GameObject[] enemies;//список префабов врагов
    [SerializeField] private float speedMin=0.5f;
    [SerializeField] private float speedMax=6f;

    private float _speed;//скорость врага
    private float _spawnPositionX;//середина платформы
    private float _spawnPositionY;//середина платформы
    private float _leftBorder;//левая граница платформы
    private float _rightBorder;//правая
    private int _enemySelector;
    private float _platformHeight;


    public void SetSpawnPosition(float spawnX, float spawnY)
    {
        _spawnPositionX = spawnX;
        _spawnPositionY = spawnY;
    }

    public void SetBorders(float platformSizeX, float platformSizeY)
    {
        _leftBorder = _spawnPositionX - platformSizeX / 2f;
        _rightBorder = _spawnPositionX + platformSizeX / 2f;
        _platformHeight = platformSizeY/2f;
    }

    public void SpawnEnemy()
    {
        _enemySelector = Random.Range(0, enemies.Length);
        _speed = Random.Range(speedMin, speedMax);
        GameObject currentEnemy = Instantiate(enemies[_enemySelector],
            new Vector3(_spawnPositionX, _spawnPositionY + _platformHeight + enemies[_enemySelector].transform.GetChild(0).GetComponent<CircleCollider2D>().radius + 0.5f,0),transform.rotation);
        currentEnemy.transform.GetChild(0).GetComponent<EnemyController>().SetEnemyParameters(_speed, _leftBorder, _rightBorder);
    }
}

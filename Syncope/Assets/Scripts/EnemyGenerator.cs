using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator:MonoBehaviour
{
    public GameObject[] enemies;//список префабов врагов
    public float speedMin;
    public float speedMax;

    private float speed;//скорость врага
    private float spawnPositionX;//середина платформы
    private float spawnPositionY;//середина платформы
    private float leftBorder;//левая граница платформы
    private float rightBorder;//правая
    private int enemySelector;
    private float platformHeight;


    public void SetSpawnPosition(float spawnX, float spawnY)
    {
        spawnPositionX = spawnX;
        spawnPositionY = spawnY;
    }

    public void SetBorders(float platformSizeX, float platformSizeY)
    {
        leftBorder = spawnPositionX - platformSizeX / 2f;
        rightBorder = spawnPositionX + platformSizeX / 2f;
        platformHeight = platformSizeY/2f;
    }

    public void SpawnEnemy()
    {
        enemySelector = Random.Range(0, enemies.Length);
        speed = Random.Range(speedMin, speedMax);
        GameObject currentEnemy = Instantiate(enemies[enemySelector],
            new Vector3(spawnPositionX,spawnPositionY+platformHeight+enemies[enemySelector].transform.GetChild(0).GetComponent<CircleCollider2D>().radius + 0.5f,0),transform.rotation);
        currentEnemy.transform.GetChild(0).GetComponent<EnemyController>().SetEnemyParameters(speed, leftBorder, rightBorder);
    }
}

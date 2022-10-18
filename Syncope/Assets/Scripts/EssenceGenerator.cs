using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceGenerator : MonoBehaviour
{
    public GameObject[] essences;
    public float essenceDelta;
    public float minHeight;//высота над платформой
    public float maxHeight;

    private int essenceSelector;
    private float offset;
    private float spawnPositionX;//середина платформы
    private float spawnPositionY;//середина платформы
    private float platformHeight;
    private float height;

    public void SetSpawnPosition(float spawnX, float spawnY)
    {
        spawnPositionX = spawnX;
        spawnPositionY = spawnY;
    }

    public void SetPlatformHeight(float platHeight)
    {
        platformHeight = platHeight / 2f;
    }

    public void SpawnEssence()
    {
        essenceSelector = Random.Range(0, essences.Length);
        offset = Random.Range(-essenceDelta, essenceDelta);
        height = Random.Range(minHeight, maxHeight);
        GameObject currentEssence = PoolManager.getGameObjectFromPool(essences[essenceSelector]);
        currentEssence.transform.position = new Vector3(spawnPositionX + offset,spawnPositionY+platformHeight+height,0);
        //currentEssence.transform.GetChild(0).GetChild(0).GetComponent<Blinking>().StartBlinking();
    }
}

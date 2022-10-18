using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapGenerator : MonoBehaviour
{
    public GameObject[] traps;//список префабов ловушек
    public float trapDelta;//смещение спавна ловушки
    public float minGravity;
    public float maxGravity;

    private float gravity;//скорость падения ловушки
    private int trapSelector;
    private float platformHeight;
    private float spawnPositionX;//середина платформы
    private float spawnPositionY;//середина платформы
    private float offset;

    public void SetSpawnPosition(float posX, float posY)
    {
        spawnPositionX = posX;
        spawnPositionY = posY;
    }

    public void SetPlatformHeight(float platHeight)
    {
        platformHeight = platHeight/2f;
    }

    public void SpawnTrap()
    {
        trapSelector = Random.Range(0,traps.Length);
        gravity = Random.Range(minGravity, maxGravity);
        offset = Random.Range(-trapDelta, trapDelta);
        GameObject currentTrap = Instantiate(traps[trapSelector],
            new Vector3(spawnPositionX+offset, spawnPositionY + platformHeight + traps[trapSelector].GetComponent<BoxCollider2D>().size.y/2f, 0), transform.rotation);
        currentTrap.transform.GetChild(0).GetComponent<TrapEntryController>().SetTrapGravity(gravity);
    }
}

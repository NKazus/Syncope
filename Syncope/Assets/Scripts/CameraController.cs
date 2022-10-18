using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float damping = 1.5f;
    public Vector2 offset = new Vector2(2f, 1f);
    public bool isLeft;

    private Transform player;
    private int lastX;
    [SerializeField] private float upperLimit;
    [SerializeField] private float bottomLimit;

    private void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x),offset.y);
        upperLimit = GameObject.FindGameObjectWithTag("MaxHeightPoint").transform.position.y;//центр камеры не может выйти за самую высокую
        bottomLimit = GameObject.FindGameObjectWithTag("PlatformGenerator").transform.position.y;//и самую низкую точку спавна платформ
        FindPlayer(isLeft);
    }

    public void FindPlayer(bool playerIsLeft)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        lastX = Mathf.RoundToInt(player.position.x);
        if (playerIsLeft)
            transform.position = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
        else
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
    }

    private void Update()
    {
        if (player)
        {
            int currentX = Mathf.RoundToInt(player.position.x);
            if (currentX > lastX)
                isLeft = false;
            else if (currentX < lastX)
                isLeft = true;
            lastX = Mathf.RoundToInt(player.position.x);

            Vector3 target;
            if(isLeft)
                target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
            else
                target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
            transform.position = currentPosition;
        }

        transform.position = new Vector3(
            transform.position.x, Mathf.Clamp(transform.position.y, bottomLimit, upperLimit), transform.position.z);
    }
}

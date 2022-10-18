using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingPoint : MonoBehaviour
{
    private PlayerController player;
    private PlatformGenerator platformGenerator;
    private float currentHealingPointPosition;
    private AudioSource healingSound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        platformGenerator = GameObject.FindGameObjectWithTag("PlatformGenerator").GetComponent<PlatformGenerator>();
        healingSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            player.setHiding(true);
            platformGenerator.setHealingDesreasing(false);//когда входит в коллайдер, останавливается отсчет спавна следующей хилки (чтобы во время захила, не шло время)
            platformGenerator.setHealingSpawnTime();//бахаем отсчет в начальное значение
            healingSound.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            player.setHiding(false);
            currentHealingPointPosition = getCurrentHealingPointPosition();//получаем координату хилки, из которой только вышли
            healingSound.Stop();
            if((currentHealingPointPosition==platformGenerator.getLastHealingPoint()) && (player.transform.position.x > currentHealingPointPosition))//если она последняя, и мы выходим вправо, запускаем отсчет
            {
                print("Spawning enabled");
                platformGenerator.setHealingDesreasing(true);
            }
        }
    }

    private float getCurrentHealingPointPosition()
    {
        print("Healing plat " + transform.root.position.x);
        return transform.root.position.x;
    }
}

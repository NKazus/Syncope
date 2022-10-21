using UnityEngine;

public class HealingPoint : MonoBehaviour
{
    private PlayerController _player;
    private PlatformGenerator _platformGenerator;
    private float _currentHealingPointPosition;
    private AudioSource _healingSound;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _platformGenerator = GameObject.FindGameObjectWithTag("PlatformGenerator").GetComponent<PlatformGenerator>();
        _healingSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _player.SetHiding(true);
            _platformGenerator.SetHealingDesreasing(false);//когда входит в коллайдер, останавливается отсчет спавна следующей хилки (чтобы во время захила, не шло время)
            _platformGenerator.SetHealingSpawnTime();//бахаем отсчет в начальное значение
            _healingSound.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Character"))
        {
            _player.SetHiding(false);
            _currentHealingPointPosition = GetCurrentHealingPointPosition();//получаем координату хилки, из которой только вышли
            _healingSound.Stop();
            if((_currentHealingPointPosition == _platformGenerator.GetLastHealingPoint()) && (_player.transform.position.x > _currentHealingPointPosition))//если она последняя, и мы выходим вправо, запускаем отсчет
            {
                _platformGenerator.SetHealingDesreasing(true);
            }
        }
    }

    private float GetCurrentHealingPointPosition()
    {
        return transform.root.position.x;
    }
}

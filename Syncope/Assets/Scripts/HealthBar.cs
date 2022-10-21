using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _bar;//изменяющееся изображение
    [SerializeField] private float _timeScale=0.02f;//стандартная скорость изменения

    private float _fill;//процентное количество хп [0,1]
    private float _fillCoefficient;//изменение от врагов и хилок [+2/-2]
    private AudioSource _essenceSound;

    private void Awake()
    {
        GlobalEventManager.HealthChangeEvent.AddListener(HealthChange);
    }

    private void Start()
    {
        _fill = 1f;
        _fillCoefficient = 1f;
        _essenceSound = transform.root.GetComponent<AudioSource>();
    }

    private void Update()
    {
        _fill = Mathf.Clamp(_fill - Time.deltaTime*_timeScale*_fillCoefficient,0f,1f);
        _bar.fillAmount = _fill;
        if (_fill <= 0)
            GlobalEventManager.RestartScene();
    }

    public float getFill()//получение хп для затухания и мб не только
    {
        return _fill;
    }

    public void setFillCoefficient(float coeff)
    {
        _fillCoefficient = coeff;
    }

    private void HealthChange(int healthCoefficient)
    {
        _fill += 1f / (float)healthCoefficient;
        if (healthCoefficient > 0) 
        {
            _essenceSound.Play();
        }
    }
}

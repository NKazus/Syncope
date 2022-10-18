using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public SceneController sceneController;
    public Image bar;//изменяющееся изображение
    public float timeScale;//стандартная скорость изменения

    private float fill;//процентное количество хп [0,1]
    private float fillCoefficient;//изменение от врагов и хилок [+2/-2]
    private AudioSource essenceSound;

    // Start is called before the first frame update
    void Start()
    {
        fill = 1f;
        fillCoefficient = 1f;
        essenceSound = transform.root.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        fill = Mathf.Clamp(fill - Time.deltaTime*timeScale*fillCoefficient,0f,1f);
        bar.fillAmount = fill;
        if (fill <= 0)
            sceneController.RestartLevel();
    }

    public float getFill()//получение хп для затухания и мб не только
    {
        return fill;
    }

    public void setFillCoefficient(float coeff)
    {
        fillCoefficient = coeff;
    }

    public void healthReduce(int affectCoefficient)
    {
        fill -= 1f / (float)affectCoefficient;
    }

    public void healthIncrease(int increaseCoefficient)
    {
        fill += 1f / (float)increaseCoefficient;
        essenceSound.Play();
    }
}

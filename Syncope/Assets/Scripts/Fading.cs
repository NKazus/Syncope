using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour
{
    public float additionCoefficient;
    public float divisionCoefficient;

    private SpriteRenderer character;
    private Color color;
    private HealthBar health;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<SpriteRenderer>();
        color = character.material.color;
        health = GetComponent<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        color.a = Mathf.Log((health.getFill()+additionCoefficient)* (health.getFill() + additionCoefficient))/divisionCoefficient;
        character.material.color = color;
    }
}

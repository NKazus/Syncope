using UnityEngine;

public class Fading : MonoBehaviour
{
    [SerializeField] private float additionCoefficient = 2f;
    [SerializeField] private float divisionCoefficient = 2.2f;

    private SpriteRenderer _character;
    private Color _color;
    private HealthBar _health;

    private void Start()
    {
        _character = GetComponent<SpriteRenderer>();
        _color = _character.material.color;
        _health = GetComponent<HealthBar>();
    }

    private void Update()
    {
        _color.a = Mathf.Log((_health.getFill()+ additionCoefficient) * (_health.getFill() + additionCoefficient))/ divisionCoefficient;
        _character.material.color = _color;
    }
}
